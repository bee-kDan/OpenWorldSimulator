using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public TMP_Text interaction_text;
    public float rayDistance = 5f;

    private InteractableObject currentInteractable;

    public Image centerDotImage;
    public Image handIcon;

    private void Start()
    {
        interaction_text.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Camera.main == null) return;

        // Bắn ray từ giữa màn hình (chỗ có dấu chấm)
        Ray ray = Camera.main.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        currentInteractable = null;
        bool showHand = false;

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            InteractableObject interactable =
                hit.transform.GetComponentInParent<InteractableObject>();

            if (interactable != null)
            {
                currentInteractable = interactable;
                interaction_text.text = interactable.GetItemName();
                interaction_text.gameObject.SetActive(true);

                // Kiểm tra tag ở cả collider trúng lẫn object cha
                showHand = hit.collider.CompareTag("pickable")
                        || interactable.gameObject.CompareTag("pickable");
            }
        }

        if (currentInteractable == null)
            interaction_text.gameObject.SetActive(false);

        centerDotImage.gameObject.SetActive(!showHand);
        handIcon.gameObject.SetActive(showHand);

        if (currentInteractable != null &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentInteractable.Pickup();
            currentInteractable = null;
            interaction_text.gameObject.SetActive(false);
            centerDotImage.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);
        }
    }
}