using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public Text inventoryCountText;
    private int occupiedSlots;
    private int inventoryCapacity;

    private void Start()
    {
        inventoryCapacity = inventorySlots.Length;
        RefreshInventoryCount();
    }

    public bool AddItem(Item item)
    {
        // Find any empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                IncreaseItemCount();

                return true;
            }
        }

        return false;
    }

    public void ReturnItem(InventoryItem inventoryItem)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                inventoryItem.transform.SetParent(slot.transform);
                IncreaseItemCount();

                return;
            }
        }
    }    

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public void DecreaseItemCount()
    {
        occupiedSlots--;
        RefreshInventoryCount(occupiedSlots);
    }

    public void IncreaseItemCount()
    {
        occupiedSlots++;
        RefreshInventoryCount(occupiedSlots);
    }

    public void RefreshInventoryCount()
    {
        occupiedSlots = 0;

        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                occupiedSlots++;
            }
        }

        RefreshInventoryCount(occupiedSlots);
    }

    private void RefreshInventoryCount(int occupiedSlots)
    {
        inventoryCountText.text = $"{occupiedSlots} / {inventoryCapacity}";
    }

    private void OnEnable()
    {
        InventorySlot.onInventoryCountIncrease += IncreaseItemCount;
        InventorySlot.onInventoryCountDecrease += DecreaseItemCount;
    }

    private void OnDisable()
    {
        InventorySlot.onInventoryCountIncrease -= IncreaseItemCount;
        InventorySlot.onInventoryCountDecrease -= DecreaseItemCount;
    }
}
