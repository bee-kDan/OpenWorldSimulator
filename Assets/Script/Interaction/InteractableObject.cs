using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string itemName;

    public string GetItemName()
    {
        return itemName;
    }
    public void Pickup()
    {
        if (!InventorySystem.Instance.CheckIsFull())
        {
            Debug.Log("Picked up: " + itemName);

            InventorySystem.Instance.AddToInventory(itemName);

            // Tạm thời xóa item khỏi scene
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Your bag is full");
        }
    }

}