using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Mouse.current == null) return;

        // Chuột trái
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            animator.SetTrigger("hit");
        }
    }
}