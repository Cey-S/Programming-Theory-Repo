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

    public void SubmitItems()
    {
        if (IsQuestGoalReached())
        {
            Debug.Log("Quest Completed!");
        }
        else
        {
            Debug.Log("Insufficient Items...");
            ReturnItems();
        }
    }

    private bool IsQuestGoalReached()
    {
        bool result = false;

        List<QuestItem> submittedItems = GetSubmittedItems();
        if (submittedItems == null)
        {
            return false;
        }

        foreach (QuestItem questItem in quests[currentQuest].questItems)
        {
            int found = submittedItems.FindIndex(submittedItem => submittedItem.Name == questItem.Name);
            if (found == -1)
            {
                return false;
            }
            else
            {
                result = submittedItems[found].Amount.Equals(questItem.Amount);

                if(result == false)
                {
                    return false;
                }
            }
        }

        return result;
    }

    private List<QuestItem> GetSubmittedItems()
    {
        List<QuestItem> submittedItems = new List<QuestItem>();
        foreach (InventorySlot slot in itemDropSlots)
        {
            if (slot.gameObject.activeInHierarchy)
            {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                //empty slot means missing quest items, no need to continue => submission is null
                if (itemInSlot == null) 
                {
                    return null;
                }

                int found = submittedItems.FindIndex(item => item.Name == itemInSlot.item.id);
                //couldn't find an entry for that quest item name so we add a new one
                if (found == -1)
                {
                    submittedItems.Add(new QuestItem()
                    {
                        Name = itemInSlot.item.id,
                        Amount = 1
                    });
                }
                else
                {
                    submittedItems[found].Amount += 1;
                }
            }
        }

        return submittedItems;
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
