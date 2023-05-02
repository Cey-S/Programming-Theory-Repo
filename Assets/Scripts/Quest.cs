using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestItem
{
    public string Name;
    public int Amount;
}

public class Quest
{
    public string title;
    public string description = "I need ";
    public Sprite personAvatar;
    public string personName;
    public int coinReward;
    public List<QuestItem> questItems = new List<QuestItem>();
    public int totalSlotsNeeded;

    public QuestGoal goal;
}

public class QuestGoal
{
    public GoalType type;
}

public enum GoalType
{
    FoodGathering,
    WoodGathering
}
