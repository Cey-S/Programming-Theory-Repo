using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIHandler : MonoBehaviour
{
    public GameObject inventoryGroup;
    bool isInventoryOpen;

    void Start()
    {
        inventoryGroup.SetActive(false);
        isInventoryOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryButton();
        }
    }

    public void InventoryButton()
    {
        if (!isInventoryOpen)
        {
            inventoryGroup.SetActive(true);
            isInventoryOpen = true;
        }
        else
        {
            inventoryGroup.SetActive(false);
            isInventoryOpen = false;
        }
    }
}
