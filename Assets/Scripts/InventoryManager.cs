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
        InitializeInventoryCount();
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
                occupiedSlots++;
                RefreshInventoryCount();

                return true;
            }
        }

        return false;
    }

    public void RemoveItem(InventoryItem inventoryItem)
    {
        occupiedSlots--;
        RefreshInventoryCount();

        Destroy(inventoryItem.gameObject);
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }    

    void InitializeInventoryCount()
    {
        int count = 0;

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].GetComponentInChildren<InventoryItem>() != null)
            {
                count++;
            }
        }

        occupiedSlots = count;
        inventoryCapacity = inventorySlots.Length;
    }

    void RefreshInventoryCount()
    {
        inventoryCountText.text = $"{occupiedSlots} / {inventoryCapacity}";
    }
}
