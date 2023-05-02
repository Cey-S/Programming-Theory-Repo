using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{    
    public int rewardMultiplier = 10;
    public Item[] items;

    private List<string> woodItems = new List<string>();
    private List<string> foodItems = new List<string>();

    private void Start()
    {
        foreach (Item item in items)
        {
            if (item.type == ItemType.Wood)
            {
                woodItems.Add(item.id);
            }
            else
            {
                foodItems.Add(item.id);
            }
        }
    }

    public Quest GetQuest()
    {
        Quest quest = new Quest(); // initialize empty quest to fill

        int randomQuestType = Random.Range(0, 2);
        
        if (randomQuestType == 0)
        {
            //quest.goal.type = GoalType.WoodGathering;

            // Set title
            quest.title = "Wood Needed!";

            // Set quest items
            GenerateQuestItems(quest, woodItems, 6);

            // Set description
            GenerateQuestDescription(quest);

            // Set reward
            GenerateQuestReward(quest);
        }
        else
        {
            //quest.goal.type = GoalType.FoodGathering;
            
            // Set title
            quest.title = "Food Needed!";

            // Set quest items
            GenerateQuestItems(quest, foodItems, 4);

            // Set description
            GenerateQuestDescription(quest);

            // Set reward
            GenerateQuestReward(quest);
        }

        return quest; // return now filled quest
    }

    private void GenerateQuestReward(Quest quest)
    {
        int reward = 0;
        foreach (QuestItem item in quest.questItems)
        {
            reward += item.Amount * rewardMultiplier;
        }
        quest.coinReward = reward;
    }

    private void GenerateQuestDescription(Quest quest)
    {
        string description = "";
        for (int i = 0; i < quest.questItems.Count; i++)
        {
            if (quest.questItems.Count != 1 && i == quest.questItems.Count - 1) // before the last item
            {
                description += " and ";
            }

            description += quest.questItems[i].Amount + " " + quest.questItems[i].Name;

            if (i + 1 < quest.questItems.Count - 1)
            {
                description += ", ";
            }
        }
        quest.description += description;
    }

    private void GenerateQuestItems(Quest quest, List<string> items, int maxNumPerItem)
    {
        int totalAmount = 0;
        for (int i = 0; i < items.Count; i++)
        {
            int amount = Random.Range(0, maxNumPerItem);
            totalAmount += amount;

            if (amount != 0 && totalAmount <= 10) // total num of inventory slots
            {
                quest.questItems.Add(new QuestItem()
                {
                    Name = items[i],
                    Amount = amount
                });
            }
            else
            {
                totalAmount -= amount; // did not add the previous item because of inventory capacity
            }
        }

        if (quest.questItems.Count == 0) // if random amount was 0 every time
        {
            int type = Random.Range(0, items.Count);
            int amount = Random.Range(1, 11);
            quest.questItems.Add(new QuestItem()
            {
                Name = items[type],
                Amount = amount
            });

            totalAmount += amount;
        }

        quest.totalSlotsNeeded = totalAmount;
    }
}
