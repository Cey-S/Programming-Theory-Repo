using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestBoard : MonoBehaviour
{
    public InventoryManager playerInventory;
    public QuestGenerator generator;

    public InventorySlot[] itemDropSlots;

    List<Quest> quests = new List<Quest>();
    int currentQuest;

    public delegate void OnQuestComplete();
    public static event OnQuestComplete SetNewQuestUI;

    private void Start()
    {
        quests.Add(generator.GetQuest());
        quests.Add(generator.GetQuest());
        //quests.Add(generator.GetQuest());

        currentQuest = 0;
    }

    // Handles how many item drop slots should there be according to the active quest's need
    public void RefreshItemSlots()
    {
        if (currentQuest == -1)
        {
            foreach (InventorySlot slot in itemDropSlots)
            {
                slot.gameObject.SetActive(false);
            }

            return;
        }

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

    private void ClearSlotItems()
    {
        foreach (InventorySlot slot in itemDropSlots)
        {
            if (slot.gameObject.activeInHierarchy)
            {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                Destroy(itemInSlot.gameObject);
            }
        }
    }

    public void SubmitItems()
    {
        if (IsQuestGoalReached())
        {
            quests[currentQuest].isCompleted = true;
            quests.RemoveAt(currentQuest);
            if (!quests.Any()) // if there is no quest left
            {
                currentQuest = -1;

            }
            else if (currentQuest.Equals(quests.Count)) // if the last quest of the quests list is completed
            {
                currentQuest = 0; // then navigate to the first quest of the list
            }

            ClearSlotItems();
            SetNewQuestUI?.Invoke();
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
        if (!submittedItems.Any())
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

                if (result == false)
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
                //empty slot means missing quest items, no need to continue
                if (itemInSlot == null)
                {
                    return submittedItems;
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
        if (currentQuest == -1)
        {
            return "Wait for new quest to appear.";
        }
        return quests[currentQuest].description;
    }

    public string GetQuestReward()
    {
        if (currentQuest == -1)
        {
            return "0";
        }
        return quests[currentQuest].coinReward.ToString();
    }

    public string GetQuestTitle()
    {
        if (currentQuest == -1)
        {
            return "No New Quest";
        }
        return quests[currentQuest].title;
    }

    public void NextQuest()
    {
        if (!quests.Any()) // if there is no quest available, return
            return;

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
        if (!quests.Any()) // if there is no quest available, return
            return;

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
