using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // --- Is this item trashable --- //
    public bool isTrashable;

    // --- Item Info UI --- //
    private GameObject itemInfoUI;

    private TMP_Text itemInfoUI_itemName;
    private TMP_Text itemInfoUI_itemDescription;
    private TMP_Text itemInfoUI_itemFunctionality;

    public string thisName, thisDescription, thisFunctionality;

    // --- Consumption --- //
    private GameObject itemPendingConsumption;
    public bool isConsumable;

    public float healthEffect;
    public float caloriesEffect;
    public float hydrationEffect;


    // --- Equipment --- //
    public bool isEquippable;
    private GameObject itemPendingEquipping;

    public bool isInsideQuickSlot;
    public bool isSelected;



    private void Start()
    {

        itemInfoUI = InventorySystem.Instance.ItemInfoUi;
        itemInfoUI_itemName = itemInfoUI.transform.Find("itemName").GetComponent<TMP_Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("itemDescription").GetComponent<TMP_Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("itemFunctionality").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (isSelected)
        {
            gameObject.GetComponent<DragDrop>().enabled = false;
        } 
        else gameObject.GetComponent<DragDrop>().enabled = true;

    }

    // Triggered when the mouse enters into the area of the item that has this script.
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hover: " + gameObject.name);
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }

    // Triggered when the mouse exits the area of the item that has this script.
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    // Triggered when the mouse is clicked over the item that has this script.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (isConsumable)
            {
                itemPendingConsumption = gameObject;
                consumingFunction(healthEffect, caloriesEffect, hydrationEffect);
            }

            if (isEquippable && !isInsideQuickSlot && EquipSystem.Instance.CheckIfFull() == false)
            {
                EquipSystem.Instance.AddToQuickSlots(gameObject);
                isEquippable = true;
            }
        }

    }

    // Triggered when the mouse button is released over the item that has this script.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (Mouse.current != null && Mouse.current.rightButton.wasReleasedThisFrame)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.RecalculateList();
                CraftingSystem.Instance.RefreshNeededItems();
            }
        }
    }

    private void consumingFunction(float healthEffect, float caloriesEffect, float hydrationEffect)
    {
        itemInfoUI.SetActive(false);

        healthEffectCalculation(healthEffect);

        caloriesEffectCalculation(caloriesEffect);

        hydrationEffectCalculation(hydrationEffect);

    }


    private static void healthEffectCalculation(float healthEffect)
    {
        // --- Health --- //

        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;

        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                PlayerState.Instance.setHealth(maxHealth);
            }
            else
            {
                PlayerState.Instance.setHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }


    private static void caloriesEffectCalculation(float caloriesEffect)
    {
        // --- Calories --- //

        float caloriesBeforeConsumption = PlayerState.Instance.currentCalories;
        float maxCalories = PlayerState.Instance.maxCalories;

        if (caloriesEffect != 0)
        {
            if ((caloriesBeforeConsumption + caloriesEffect) > maxCalories)
            {
                PlayerState.Instance.setCalories(maxCalories);
            }
            else
            {
                PlayerState.Instance.setCalories(caloriesBeforeConsumption + caloriesEffect);
            }
        }
    }


    private static void hydrationEffectCalculation(float hydrationEffect)
    {
        // --- Hydration --- //

        float hydrationBeforeConsumption = PlayerState.Instance.currentHydrationPercent;
        float maxHydration = PlayerState.Instance.maxHydrationPercent;

        if (hydrationEffect != 0)
        {
            if ((hydrationBeforeConsumption + hydrationEffect) > maxHydration)
            {
                PlayerState.Instance.setHydration(maxHydration);
            }
            else
            {
                PlayerState.Instance.setHydration(hydrationBeforeConsumption + hydrationEffect);
            }
        }
    }


}