using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public bool isOpen;

    public List <GameObject> slotList = new List<GameObject>();
    public List <string> itemList = new List<string>();
    //public bool isFull;

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;

        PopulateSlotList();
    }


    // add all the slot in the inventory to the list 
    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform) 
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }



    void Update()
    {

        if (Keyboard.current.iKey.isPressed && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Keyboard.current.iKey.isPressed && isOpen)
        {
            if (!CraftingSystem.Instance.isOpen)
            {
                inventoryScreenUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                isOpen = false;
            }
        }
    }

    public void AddToInventory(string itemName)
    {

            whatSlotToEquip = FindNextEmptySlot();

            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);

            itemToAdd.transform.SetParent(whatSlotToEquip.transform);

            itemList.Add(itemName);
        
    }

    public bool CheckIsFull()
    {
        int counter = 0;
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }


        if (counter == 21)
        {
            return true;
        }
        else return false; 

    }

    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in slotList)
        {
            if (slot.transform.childCount ==0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    // Hàm remove 1 item trong slotList (tên, SL)
    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for (int i = slotList.Count - 1; i >= 0 && counter > 0; i--)
        {
            Transform slot = slotList[i].transform;

            if (slot.childCount > 0)
            {
                GameObject item = slot.GetChild(0).gameObject;

                string itemName = item.name.Replace("(Clone)", "").Trim();

                if (itemName == nameToRemove)
                {
                    Destroy(item);
                    counter--;
                }
            }
        }

        RecalculateList();
    }

    // Hàm đếm item trong túi để hiển thị khi craft xem mình có đủ DK để craft ko 
    public void RecalculateList()
    {
        itemList.Clear();
        foreach(GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string cloneText = "Clone";
                string clearName = name.Replace(cloneText, "");
                itemList.Add(clearName);
            }

        }
    }

    


} 