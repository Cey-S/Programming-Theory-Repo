using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public InventoryManager playerInventory;

    private Transform otherSlotParent;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            otherSlotParent = inventoryItem.parentAfterDrag.parent;
            inventoryItem.parentAfterDrag = transform;

            // if the item's grandparent changed between inventory and quest board 
            if (!otherSlotParent.Equals(transform.parent))
            {
                // if the previous parent belongs to the inventory group
                if (otherSlotParent.CompareTag("Inventory"))
                {
                    playerInventory.DecreaseItemCount();
                }
                else if (transform.parent.CompareTag("Inventory")) // if the item is dropped into the inventory slot
                {
                    playerInventory.IncreaseItemCount();
                }

            }

        }
    }
}
