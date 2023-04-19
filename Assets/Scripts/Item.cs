using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{
    public string id;
    public ItemType type;
    public Sprite image;
    public GameObject prefab;
    public bool stackable = true;
}

public enum ItemType
{
    Food,
    Wood
}
