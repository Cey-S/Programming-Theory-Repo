using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainUIHandler : MonoBehaviour
{
    [Header("Inventory UI")]
    public GameObject inventoryGroup;
    bool isInventoryOpen;

    [Header("Tree UI")]
    public GameObject treeInfoPopUpGroup;
    public Text treeName;
    public Text productionInfo;
    public Text productionCapacity;
    public Text productName;
    public Image productIcon;
    public Text productCount;
    bool isTreeInfoOpen;

    Tree selectedTree;

    [Header("Quest UI")]
    public GameObject questBoardGroup;
    public Text questTitle;
    public Text questDescription;
    public Text questReward;
    [Space]
    public QuestBoard questBoard;

    public void GoToNextQuest()
    {
        questBoard.NextQuest();
        SetQuestBoardContent();
    }

    public void GoToPreviousQuest()
    {
        questBoard.PrevQuest();
        SetQuestBoardContent();
    }

    public void SetQuestBoardContent()
    {
        questTitle.text = questBoard.GetQuestTitle();
        questDescription.text = questBoard.GetQuestDescription();
        questReward.text = questBoard.GetQuestReward();

        questBoard.RefreshItemSlots();
    }

    public interface ITreeInfoContent
    {
        string GetTreeName();
        string GetProductionInfo();
        string GetProductionCapacity();
        string GetProductName();
        Sprite GetProductIcon();

        //string GetProductCount();
    }
    
    void Start()
    {
        inventoryGroup.SetActive(false);
        isInventoryOpen = false;

        treeInfoPopUpGroup.SetActive(false);

        questBoardGroup.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryButton();
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleSelection();
        }

        if (isTreeInfoOpen)
        {
            productCount.text = selectedTree.Inventory.Count == 0 ? "0" : selectedTree.Inventory[0].Count.ToString();
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

    public void HandleSelection()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the mouse was clicked over a UI element
            bool isOverUI = EventSystem.current.IsPointerOverGameObject();
            if (!isOverUI)
            {
                if (hit.transform.CompareTag("QuestBoard"))
                {
                    questBoardGroup.SetActive(true);
                    SetQuestBoardContent();
                }
                else //if (hit.transform.CompareTag("Tree"))
                {
                    questBoardGroup.SetActive(false);
                }

                //var tree = hit.collider.GetComponent<Tree>();
                selectedTree = hit.collider.GetComponent<Tree>();

                if (selectedTree != null)
                {
                    treeInfoPopUpGroup.SetActive(true);
                    isTreeInfoOpen = true;

                    ITreeInfoContent treeInfo = selectedTree.GetComponent<ITreeInfoContent>();
                    SetTreeInfoContent(treeInfo);
                }
                else
                {
                    treeInfoPopUpGroup.SetActive(false);
                    isTreeInfoOpen = false;
                }
            }
        }
        else
        {
            questBoardGroup.SetActive(false);
            treeInfoPopUpGroup.SetActive(false);
            isTreeInfoOpen = false;
        }
    }

    void SetTreeInfoContent(ITreeInfoContent treeInfo)
    {
        treeName.text = treeInfo.GetTreeName();
        productionInfo.text = treeInfo.GetProductionInfo();
        productionCapacity.text = treeInfo.GetProductionCapacity();
        productName.text = treeInfo.GetProductName();
        productIcon.sprite = treeInfo.GetProductIcon();
    }
}
