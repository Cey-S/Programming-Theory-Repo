using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoard : MonoBehaviour, MainUIHandler.IQuestInfoContent
{
    public QuestGenerator generator;

    List<Quest> quests = new List<Quest>();

    private void Start()
    {
        quests.Add(generator.GetQuest());
    }

    public string GetQuestDescription()
    {
        return quests[0].description;
    }

    public string GetQuestReward()
    {
        return quests[0].coinReward.ToString();
    }

    public string GetQuestTitle()
    {
        return quests[0].title;
    }

}
