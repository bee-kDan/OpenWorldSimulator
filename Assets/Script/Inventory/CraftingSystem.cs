using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;



public class CraftingSystem : MonoBehaviour
{

    public GameObject craftingSceneUI;
    public GameObject toolsSceneUI;
    
    public List<string> inventoryItemList = new List<string>();

    // Category Button
   Button toolsBTN;

    // Craft Button
    Button craftAxeBTN;

    // Requirement Text
    TextMeshProUGUI AxeReq1, AxeReq2;

    public bool isOpen;

    public CraftableBlueprint AxeBLP = new CraftableBlueprint("Axe","Stone", "Stick", 3,3,2);

    public static CraftingSystem Instance { get; set; }

    public void Awake()
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

        toolsBTN = craftingSceneUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate{ OpenToolCategory();  });

        // Axe

        AxeReq1 = toolsSceneUI.transform.Find("Axe").transform.Find("req_1").GetComponent<TextMeshProUGUI>();
          
        AxeReq2 = toolsSceneUI.transform.Find("Axe").transform.Find("req_2").GetComponent<TextMeshProUGUI>();

        craftAxeBTN = toolsSceneUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

    }

    // Update is called once per frame
    void Update()
    {
        // Gọi hàm để update UI Crafting
        RefreshNeededItems();

        // Mở Inventory
        if (Keyboard.current.cKey.isPressed && !isOpen)
        {

            Debug.Log("i is pressed");
            craftingSceneUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        // Mở Crafting UI
        else if (Keyboard.current.cKey.isPressed && isOpen)
        {
            craftingSceneUI.SetActive(false);
            toolsSceneUI.SetActive(false);
            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                isOpen = false;
            }
        }
         
    }

    void OpenToolCategory()
    {
        craftingSceneUI.SetActive(false);
        toolsSceneUI.SetActive(true);
    }

    void CraftAnyItem(CraftableBlueprint bluePrintToCraft)
    {
        if (InventorySystem.Instance == null)
        {
            Debug.LogError("InventorySystem.Instance is NULL!");
            return;
        }

        if (bluePrintToCraft == null)
        {
            Debug.LogError("bluePrintToCraft is NULL!");
            return;
        }

        // Kiểm tra nguyên liệu trước
        if (bluePrintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(
                bluePrintToCraft.Req2,
                bluePrintToCraft.Req1Ammount
            );
        }
        else if (bluePrintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(
                bluePrintToCraft.Req1,
                bluePrintToCraft.Req1Ammount
            );

            InventorySystem.Instance.RemoveItem(
                bluePrintToCraft.Req2,
                bluePrintToCraft.Req2Ammount
            );
        }

        // Sau khi remove nguyên liệu mới thêm item
        InventorySystem.Instance.AddToInventory(bluePrintToCraft.itemName);

        StartCoroutine(calculate());

        RefreshNeededItems();
    }

    public IEnumerator calculate()
    {
        yield return new WaitForSeconds(1);

        InventorySystem.Instance.RecalculateList();
    }

    private void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch(itemName){

                case "Stone":
                    stone_count++;
                    break;

                case "Stick":
                    stick_count++;
                    break;

            }

        } 
        //-----AXE-------

        AxeReq1.text="3 Stone [" + stone_count + "]";
        AxeReq2.text = "3 Stick [" + stick_count + "]";


        if (stone_count >=3 && stick_count >= 3)
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }
    }

}
