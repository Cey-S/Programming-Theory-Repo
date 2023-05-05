using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    public InventoryManager playerInventory;
    public QuestGenerator generator;

    public InventorySlot[] itemDropSlots;

    List<Quest> quests = new List<Quest>();
    int currentQuest;

    private void Start()
    {
        quests.Add(generator.GetQuest());
        quests.Add(generator.GetQuest());
        quests.Add(generator.GetQuest());

        currentQuest = 0;
    }

    // Handles how many item drop slots should there be according to the active quest's need
    public void RefreshItemSlots()
    {
        int currentSlot = 0;
        foreach (InventorySlot slot in itemDropSlots)
        {
            if (currentSlot < quests[currentQuest].totalSlotsNeeded)
            {
                slot.gameObject.SetActive(true);
            }
            else
            {
                slot.gameObject.SetActive(false);
            }

            currentSlot++;
        }
    }

    // When the player closes the quest board UI or navigates between quests,
    // the items on the quest board slots return to player's inventory
    public void ReturnItems()
    {
        foreach (InventorySlot slot in itemDropSlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            // if there is an item inside the slot, return it to player's inventory
            if (itemInSlot != null)
            {
                playerInventory.ReturnItem(itemInSlot);
            }
        }
    }

    public string GetQuestDescription()
    {
        return quests[currentQuest].description;
    }

    public string GetQuestReward()
    {
        return quests[currentQuest].coinReward.ToString();
    }

    public string GetQuestTitle()
    {
        return quests[currentQuest].title;
    }

    public void NextQuest()
    {
        if (currentQuest < quests.Count)
        {
            currentQuest++;
        }

        if (currentQuest == quests.Count) // end of the list, go back to the first quest
        {
            currentQuest = 0;
        }

        ReturnItems();
    }

    public void PrevQuest()
    {
        if (currentQuest > -1)
        {
            currentQuest--;
        }

        if (currentQuest == -1)
        {
            currentQuest = quests.Count - 1;
        }

        ReturnItems();
    }
}
