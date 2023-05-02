using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    public QuestGenerator generator;

    List<Quest> quests = new List<Quest>();
    int currentQuest;

    private void Start()
    {
        quests.Add(generator.GetQuest());
        quests.Add(generator.GetQuest());
        quests.Add(generator.GetQuest());

        currentQuest = 0;
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
    }
}
