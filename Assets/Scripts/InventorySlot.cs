using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public delegate void OnInventoryCountIncrease();
    public static event OnInventoryCountIncrease onInventoryCountIncrease;

    public delegate void OnInventoryCountDecrease();
    public static event OnInventoryCountDecrease onInventoryCountDecrease;

    private Transform otherSlotParent;
    private Transform currentSlotParent;
    private bool isParentInventory;

    private void Start()
    {
        currentSlotParent = transform.parent;
        isParentInventory = currentSlotParent.CompareTag("Inventory");
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            otherSlotParent = inventoryItem.parentAfterDrag.parent;

            // if the item's grandparent changed between inventory and quest board 
            if (!otherSlotParent.Equals(currentSlotParent))
            {
                // if the item is dropped into the inventory slot
                if (isParentInventory) 
                {
                    onInventoryCountIncrease?.Invoke();
                }
                // if the previous parent belongs to the inventory group
                else if (otherSlotParent.CompareTag("Inventory"))
                {
                    onInventoryCountDecrease?.Invoke();
                }
            }

            inventoryItem.parentAfterDrag = transform;
        }
    }
}
