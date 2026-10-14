using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public bool isOpen;

    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    //Pickup Pop up
    public GameObject pickupAlert;
    public TMP_Text pickupName;
    public Image pickupImage;

    // item Info UI
    public GameObject ItemInfoUi;

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

    // Tìm tất cả slot ở mọi cấp con (kể cả nằm trong SlotWrapper)
    private void PopulateSlotList()
    {
        slotList.Clear();

        // true = lấy cả object đang tắt (inventory thường đang inactive lúc Start)
        foreach (Transform t in inventoryScreenUI.GetComponentsInChildren<Transform>(true))
        {
            if (t.gameObject != inventoryScreenUI && t.CompareTag("Slot"))
            {
                slotList.Add(t.gameObject);
            }
        }
    }

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame && !isOpen)
        {
            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isOpen = true;
        }
        else if (Keyboard.current.iKey.wasPressedThisFrame && isOpen)
        {
            if (!CraftingSystem.Instance.isOpen)
            {
                inventoryScreenUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isOpen = false;
            }
        }
    }

    public void AddToInventory(string itemName)
    {
        whatSlotToEquip = FindNextEmptySlot();

        if (whatSlotToEquip == null)
        {
            Debug.LogWarning("Inventory đầy hoặc không tìm thấy slot nào (kiểm tra tag 'Slot')");
            return;
        }

        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);

        TriggerPickupPopup(itemName, itemToAdd.GetComponent<Image>().sprite);

        RecalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    void TriggerPickupPopup(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;
    }

    public bool CheckIsFull()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return false;
            }
        }
        return true;
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }

    // Hàm remove item trong slotList (tên, SL)
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
                    // Tách khỏi slot ngay để childCount về 0 trước khi Destroy cuối frame
                    item.transform.SetParent(null);
                    Destroy(item);
                    counter--;
                }
            }
        }

        RecalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    // Đếm item trong túi để hiển thị khi craft
    public void RecalculateList()
    {
        itemList.Clear();
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string clearName = slot.transform.GetChild(0).name.Replace("(Clone)", "").Trim();
                itemList.Add(clearName);
            }
        }
    }
}