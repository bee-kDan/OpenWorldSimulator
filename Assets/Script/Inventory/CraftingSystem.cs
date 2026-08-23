using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;



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

        AxeReq1 = toolsSceneUI.transform.Find("Axe").transform.Find("req1").GetComponent<TextMeshProUGUI>();

        AxeReq2 = toolsSceneUI.transform.Find("Axe").transform.Find("req2").GetComponent<TextMeshProUGUI>();

        craftAxeBTN = toolsSceneUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(); });

    }

    // Update is called once per frame
    void Update()
    {

        if (Keyboard.current.cKey.isPressed && !isOpen)
        {

            Debug.Log("i is pressed");
            craftingSceneUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Keyboard.current.cKey.isPressed && isOpen)
        {
            craftingSceneUI.SetActive(false);
            toolsSceneUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
         
    }

    void OpenToolCategory()
    {
        craftingSceneUI.SetActive(false);
        toolsSceneUI.SetActive(true);
    }

    void CraftAnyItem() {

    }
}
