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
    public int coinReward = 0;
    public List<QuestItem> questItems = new List<QuestItem>();
    public int totalSlotsNeeded;
    public bool isCompleted = false;
}
