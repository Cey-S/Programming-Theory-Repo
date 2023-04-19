using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tree : MonoBehaviour, MainUIHandler.ITreeInfoContent
{
    public Item product; // item to be picked by the player
    
    [SerializeField] GameObject interactionTextPrefab; // text to display when player is in range of the tree
    [SerializeField] Transform parentCanvas;
    GameObject interactionText;

    // Tree's inventory
    [System.Serializable]
    public class InventoryEntry
    {
        public string ResourceId;
        public int Count;
    }

    [Tooltip("-1 is infinite")]
    public int InventorySpace = -1;

    protected List<InventoryEntry> m_Inventory = new List<InventoryEntry>();
    public List<InventoryEntry> Inventory => m_Inventory;

    protected int m_CurrentAmount = 0;

    protected bool produceItem = true;

    //return 0 if everything fit in the inventory, otherwise return the left over amount
    public int AddItem(string resourceId, int amount)
    {
        //as we use the shortcut -1 = infinite amount, we need to actually set it to max value for computation following
        int maxInventorySpace = InventorySpace == -1 ? Int32.MaxValue : InventorySpace;

        if (m_CurrentAmount == maxInventorySpace)
        {
            produceItem = false; // stop producing
            return amount;
        }

        int found = m_Inventory.FindIndex(item => item.ResourceId == resourceId);
        int addedAmount = Mathf.Min(maxInventorySpace - m_CurrentAmount, amount);

        //couldn't find an entry for that resource id so we add a new one.
        if (found == -1)
        {
            m_Inventory.Add(new InventoryEntry()
            {
                Count = addedAmount,
                ResourceId = resourceId
            });
        }
        else
        {
            m_Inventory[found].Count += addedAmount;
        }

        m_CurrentAmount += addedAmount;
        return amount - addedAmount;
    }

    //return how much was actually removed, will be 0 if couldn't get any.
    public int GetItem(string resourceId, int requestAmount)
    {
        int found = m_Inventory.FindIndex(item => item.ResourceId == resourceId);

        //couldn't find an entry for that resource id so we add a new one.
        if (found != -1)
        {
            int amount = Mathf.Min(requestAmount, m_Inventory[found].Count);
            m_Inventory[found].Count -= amount;

            if (m_Inventory[found].Count == 0)
            {//no more of that resources, so we remove it
                m_Inventory.RemoveAt(found);
            }

            m_CurrentAmount -= amount;
            produceItem = true; // continue to produce

            return amount;
        }

        return 0;
    }

    public void PlayerInRange()
    {
        if (interactionText != null)
        {
            interactionText.SetActive(true);
        }
        else
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            interactionText = Instantiate(interactionTextPrefab, screenPos, Quaternion.identity, parentCanvas);
            interactionText.GetComponent<Text>().text = $"[F] Pick up {product.id}";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInRange();
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        interactionText.transform.position = screenPos;
    }

    private void OnTriggerExit(Collider other)
    {
        interactionText.SetActive(false);
    }

    public string GetTreeName()
    {
        return gameObject.name;
    }

    public virtual string GetProductionInfo()
    {
        return "";
    }

    public virtual string GetProductionCapacity()
    {
        return "";
    }

    public string GetProductName()
    {
        return $"Product: {product.id}";
    }

    public Sprite GetProductIcon()
    {
        return product.image;
    }    
}
