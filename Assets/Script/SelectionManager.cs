using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public TMP_Text interaction_text;
    public float rayDistance = 5f;

    private InteractableObject currentInteractable;

    private void Start()
    {
        interaction_text.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Mouse.current == null || Camera.main == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        currentInteractable = null;

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            InteractableObject interactable =
                hit.transform.GetComponent<InteractableObject>();

            if (interactable != null)
            {
                currentInteractable = interactable;

                interaction_text.gameObject.SetActive(true);
            }
            else
            {
                interaction_text.gameObject.SetActive(false);
            }
        }
        else
        {
            interaction_text.gameObject.SetActive(false);
        }

        // Nhấn E để pickup
        if (currentInteractable != null &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentInteractable.Pickup();

            currentInteractable = null;
            interaction_text.gameObject.SetActive(false);
        }
    }
}