using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Tree
{

    private void Start()
    {
        AddItem(product.id, InventorySpace);
    }

    public override string GetProductionInfo()
    {
        return "This tree has a finite amount of resources";
    }

    public override string GetProductionCapacity()
    {
        return $"Maximum capacity: {InventorySpace}";
    }
}
