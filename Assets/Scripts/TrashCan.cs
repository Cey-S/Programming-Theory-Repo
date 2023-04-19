using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCan : MonoBehaviour, IDropHandler
{
    public InventoryManager inventoryManager;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        inventoryItem.parentAfterDrag = transform;

        inventoryManager.RemoveItem(inventoryItem);
    }
}
