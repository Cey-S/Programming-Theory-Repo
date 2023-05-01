using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    private Quest quest = new Quest();

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
        int randomQuestType = Random.Range(0, 2);
        
        if (randomQuestType == 0)
        {
            //quest.goal.type = GoalType.WoodGathering;

            // Set title
            quest.title = "Wood Needed!";

            // Set quest items
            for (int i = 0; i < woodItems.Count; i++)
            {
                int amount = Random.Range(0, 6); // 6 because of 10 inventory slots
                if(amount != 0)
                {
                    quest.questItems.Add(new QuestItem()
                    {
                        Name = woodItems[i],
                        Amount = amount
                    });
                }
            }

            if (quest.questItems.Count == 0) // if random amount was 0 every time
            {
                int type = Random.Range(0, woodItems.Count);
                int amount = Random.Range(1, 11);
                quest.questItems.Add(new QuestItem()
                {
                    Name = woodItems[type],
                    Amount = amount
                });
            }

            // Set description
            string description = quest.questItems.Count == 1 ? $"{quest.questItems[0].Amount} {quest.questItems[0].Name}"
                : $"{quest.questItems[0].Amount} {quest.questItems[0].Name} and {quest.questItems[1].Amount} {quest.questItems[1].Name}";
            
            quest.description += description;

            // Set reward
            int reward = 0;
            foreach (QuestItem item in quest.questItems)
            {
                reward += item.Amount * rewardMultiplier;
            }
            quest.coinReward = reward;
        }
        else
        {
            //quest.goal.type = GoalType.FoodGathering;
            
            // Set title
            quest.title = "Food Needed!";

            int totalAmount = 0;
            // Set quest items
            for (int i = 0; i < foodItems.Count; i++)
            {
                int amount = Random.Range(0, 4);
                totalAmount += amount;

                if (amount != 0 && totalAmount <= 10) // total num of inventory slots
                {
                    quest.questItems.Add(new QuestItem()
                    {
                        Name = foodItems[i],
                        Amount = amount
                    });
                }
                else
                {
                    totalAmount -= amount; // did not add the previous item
                }
            }

            if (quest.questItems.Count == 0) // if random amount was 0 every time
            {
                int type = Random.Range(0, foodItems.Count);
                int amount = Random.Range(1, 11);
                quest.questItems.Add(new QuestItem()
                {
                    Name = foodItems[type],
                    Amount = amount
                });
            }

            // Set description
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

            // Set reward
            int reward = 0;
            foreach (QuestItem item in quest.questItems)
            {
                reward += item.Amount * rewardMultiplier;
            }
            quest.coinReward = reward;
        }

        return quest;
    }

    private void GenerateWoodQuest()
    {

    }
}
