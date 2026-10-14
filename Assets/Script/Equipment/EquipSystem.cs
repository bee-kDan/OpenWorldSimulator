using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();

    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;



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


    private void Start()
    {
        PopulateSlotList();
    }


    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame) SelectQuickSlot(1);
        else if (Keyboard.current.digit2Key.wasPressedThisFrame) SelectQuickSlot(2);
        else if (Keyboard.current.digit3Key.wasPressedThisFrame) SelectQuickSlot(3);
        else if (Keyboard.current.digit4Key.wasPressedThisFrame) SelectQuickSlot(4);
        else if (Keyboard.current.digit5Key.wasPressedThisFrame) SelectQuickSlot(5);
        else if (Keyboard.current.digit6Key.wasPressedThisFrame) SelectQuickSlot(6);
        else if (Keyboard.current.digit7Key.wasPressedThisFrame) SelectQuickSlot(7);
    }

    public void SelectQuickSlot(int number)
    {
        if (!checkIfSlotIsFull(number)) return;

        // Unselect the previous item
        if (selectedItem != null)
        {
            InventoryItem oldItem = selectedItem.GetComponent<InventoryItem>();
            if (oldItem != null) oldItem.isSelected = false;
        }

        if (selectedNumber != number)
        {
            selectedNumber = number;
            selectedItem = getSelectedItem(number);

            InventoryItem newItem = selectedItem.GetComponent<InventoryItem>();
            if (newItem == null)
            {
                Debug.LogError("No InventoryItem on " + selectedItem.name, selectedItem);
                return;
            }
            newItem.isSelected = true;

            SetAllNumberColors(Color.gray);

            Transform numberObj = numbersHolder.transform.Find("number" + number);
            if (numberObj == null)
            {
                Debug.LogError("Can't find number" + number + " under numbersHolder");
                return;
            }
            TMP_Text txt = numberObj.Find("text").GetComponent<TMP_Text>();
            txt.color = Color.white;
        }
        else
        {
            // Pressed the same slot again: deselect
            selectedNumber = -1;
            selectedItem = null;
            SetAllNumberColors(Color.gray);
        }
    }

    private void SetAllNumberColors(Color color)
    {
        if (numbersHolder == null)
        {
            Debug.LogError("numbersHolder chưa được gán trên EquipSystem", this);
            return;
        }

        foreach (Transform child in numbersHolder.transform)
        {
            Transform t = child.Find("text");
            if (t != null) t.GetComponent<TMP_Text>().color = color;
        }
    }

    GameObject getSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber - 1].transform.GetChild(0).gameObject;
    }


    bool checkIfSlotIsFull(int slotNumber)
    {
        if (quickSlotsList[slotNumber-1].transform.childCount > 0)
        {
            return true;
        }
        else return false;

    }


    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);

        InventorySystem.Instance.RecalculateList();

    }


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}