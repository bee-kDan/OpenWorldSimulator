using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;


    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        startPosition = transform.position;
        // to remember the slot that the item was in 
        startParent = transform.parent; 
        // when dragging out the item become root bc it's dont belong to the slot that was belong before
        transform.SetParent(transform.root);

        itemBeingDragged = gameObject;

    }

    public void OnDrag(PointerEventData eventData)
    {
        // make the item when drag move as fast as the mouse 
        // because in UI scale could be diff vs in game
        rectTransform.anchoredPosition += eventData.delta ;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;

        if (transform.parent == startParent || transform.parent== transform.root)
        {

            transform.position = startPosition;
            transform.SetParent(startParent);

        }

        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
