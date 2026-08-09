using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public TMP_Text interaction_text;

    private void Start()
    {
        interaction_text.gameObject.SetActive(false);
    }

    void Update()
    {

        if (Mouse.current == null || Camera.main == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Hit: " + hit.transform.name);

            InteractableObject interactable =
                hit.transform.GetComponent<InteractableObject>();

            if (interactable != null)
            {
                interaction_text.text = interactable.GetItemName();
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
    }
}