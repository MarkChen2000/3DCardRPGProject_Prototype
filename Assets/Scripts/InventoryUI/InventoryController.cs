using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryController : MonoBehaviour
{
    private CardDeckController CardDeck_Con;
    private EquipmentSlotController EquipSlot_Con;

    private Transform Trans_InventoryPanelBG;
    private TMP_Text TMP_PageNum;

    public GameObject CardTemplatePrefab; // the template that display on the panel

    private Transform Trans_InvCardsGridGroup; // locattion of the InvCardGridGroup
    private List<InventoryCard_DataLoaderAndDisplay> TemplateComponent = new List<InventoryCard_DataLoaderAndDisplay>(); // Each Template Component which is Display 

    [Space]
    public int TemplateNum = 10; // the amount of card tmeplate can display in one page
    private int CurrentPage = 1;
    private int MaxPage = 1;

    [Space]
    public CardList _InvCardListAsset; // InventoryController_CardList, the CardList-type asset
    public List<CardData> InvCardList = new List<CardData>(); // CardList that store at this script


    private void Awake()
    {
        EquipSlot_Con = GetComponent<EquipmentSlotController>();
        CardDeck_Con = GetComponent<CardDeckController>();

        Trans_InventoryPanelBG = transform.GetChild(0).GetChild(0);
        Trans_InvCardsGridGroup = Trans_InventoryPanelBG.GetChild(1).GetChild(0);
        TMP_PageNum = Trans_InventoryPanelBG.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Text>();

        if (_InvCardListAsset == null) _InvCardListAsset = Resources.Load<CardList>("CardLists_SO/Testing_InventoryCardList");
        // Load initial InvCardList asset first, this may be replaced by Save and Load System before build!
    }

    private void Start()
    {
        InitializeLoadinData();
        SpawnAllTemplateAndGetCom(); // Spawn template and get component.
        DisplayAllCardtoInventory(); // Renew Display the Current page of Cards.
        UpdatePageNum();
    }

    private void Update()
    { 
    }

    private void InitializeLoadinData()
    {
        // InvCardList = _InvCardListAsset._CardList; // only copy it's reference.

        InvCardList.Clear();
        for (int i = 0; i < _InvCardListAsset._CardList.Count; i++) // copy the value.
        {
            InvCardList.Add(_InvCardListAsset._CardList[i]);
        }
    }

    public void SaveandLoadInvCardList(bool SorL)
    {
        // save and load system.
        if (SorL)
        { }
        else
        { }
    }

    private void SortListinAssetinInvList() // Sort the cards in the list.
    {
        if (InvCardList.Count == 0)
        {
            Debug.Log("Didnt have card inside the CardLIst !");
        }
        else
        {
            // Because Carddata-type data is not the regular type like int or string that can be sorted defaulty.
            // Have to use "IComparble" to compare the data (CardCost) of elements in list, with self-made sort function.
            InvCardList.Sort(); // Sort by the Cost of Card.
        }
    }

    private int GetDiffCardNuminInvList() // Get how many DIFFERENT Card in the list;
    {
        if (InvCardList.Count == 0) return 0;

        SortListinAssetinInvList();
        int differnrtcount = 1;
        for (int i = 0; i < InvCardList.Count - 1; i++)
        {
            if (InvCardList[i] != InvCardList[i + 1])
            {
                differnrtcount++;
            }
        }
        return differnrtcount;
    }

    private void UpdatePageNum()
    {
        if (InvCardList == null)
        {
            TMP_PageNum.text = "1/X";
            return; // if there is no list data, return.
        }

        if (InvCardList.Count == 0) MaxPage = 1; // if there is a list but dont have any data in it, Set MaxPage = 1.
        else MaxPage = (int)Mathf.Ceil((float)GetDiffCardNuminInvList() / (float)TemplateNum); // �L����i�� maxpage

        if (CurrentPage > MaxPage) CurrentPage = MaxPage; // Sometime due to template change make maxpage lower then currentpage, so make currentpage equal maxpage. 

        TMP_PageNum.text = CurrentPage.ToString() + "/" + MaxPage.ToString();

        ControlTemplate(CurrentPage);
    }

    public void TurnPage(bool R) // R mean turn right, vice versa.
    {
        if (R) CurrentPage = Mathf.Clamp(CurrentPage + 1, 1, MaxPage);
        else CurrentPage = Mathf.Clamp(CurrentPage - 1, 1, MaxPage);

        DisplayAllCardtoInventory();
        UpdatePageNum();
    }

    private void SpawnAllTemplateAndGetCom() // Spawn templates, which number is the number of differnet card in list.
    {
        if (GetDiffCardNuminInvList() == 0) return;
        for (int i = 0; i < GetDiffCardNuminInvList(); i++)
        {
            SpawnOneTemplateAndGetCom();
        }
    }

    private void SpawnOneTemplateAndGetCom()
    {
        GameObject gb = Instantiate(CardTemplatePrefab, Trans_InvCardsGridGroup);
        TemplateComponent.Add(gb.GetComponent<InventoryCard_DataLoaderAndDisplay>());
    }

    private void DisplayAllCardtoInventory()
    {
        if (GetDiffCardNuminInvList() == 0) return;

        int cardnumcount = 1;
        for (int i = 0, k = 0; i < GetDiffCardNuminInvList() ;k++) // i as template count, k as the count of elements in list.
        {
            if ( i != GetDiffCardNuminInvList()-1 )
            {
                if (InvCardList[k] == InvCardList[k + 1]) // if two near element is same, then counter +1.
                {
                    cardnumcount++;
                }
                else 
                {
                    TemplateComponent[i].HoldNum = cardnumcount;
                    TemplateComponent[i].DisplaytoTemplate(InvCardList[k]);
                    i++;
                    cardnumcount = 1;
                }
            }
            else // if it is the last different card in the list.
            {
                TemplateComponent[i].HoldNum = InvCardList.Count - k ;
                TemplateComponent[i].DisplaytoTemplate(InvCardList[k]);
                i++;
            }
        }
    }

    private void ControlTemplate(int page) // Control Template Active or InActive
    {
        for (int j = 0; j < TemplateComponent.Count; j++)
        {
            if ( j >= (page-1)*TemplateNum && j< page*TemplateNum )
            {
                TemplateComponent[j].gameObject.SetActive(true);
            }
            else
            {
                TemplateComponent[j].gameObject.SetActive(false);
            }
        }
    }

    public void TryTransferCard(InventoryCard_DataLoaderAndDisplay card_template) 
        // Try to transfer card from this list to another list,
        // need to get the component of cardt template, and get the carddata and holdnum from it.
    {
        switch (card_template._CardData._CardType) // check which cardtype is the card in template.
        {
            case CardType.Spells:
                if (CardDeck_Con.ReceiveCard(card_template._CardData)) // if it is success to transfer card, check deck_con if it is ok to transfer.
                {
                    if (card_template.HoldNum > 1) // it means that doesn't need to remove the template, but remove one of the carddata from list. 
                    {
                        InvCardList.Remove(card_template._CardData);
                        WhenInvListChange(false);
                    }
                    else if (card_template.HoldNum == 1)
                    {
                        int tem_pos;
                        InvCardList.Remove(card_template._CardData);
                        tem_pos = card_template.transform.GetSiblingIndex(); // Get the position under parent in the hierarchy, which is also equal the component index in the list.
                        Destroy(card_template.gameObject);
                        TemplateComponent.RemoveAt(tem_pos); // and remove the component at that index.
                        WhenInvTemplateChange(false);
                    }
                }
                else // if it is fail, 
                {
                    Debug.Log("Fail to Transfer Spell Card!"); // Have to display the message to player.
                }
                break;

            case CardType.Equipment :
                if (EquipSlot_Con.ReceiveCard(card_template._CardData))
                {
                    if (card_template.HoldNum > 1)
                    {
                        InvCardList.Remove(card_template._CardData);
                        WhenInvListChange(false);
                    }
                    else if (card_template.HoldNum == 1)
                    {
                        int tem_pos;
                        InvCardList.Remove(card_template._CardData);
                        tem_pos = card_template.transform.GetSiblingIndex();
                        Destroy(card_template.gameObject);
                        TemplateComponent.RemoveAt(tem_pos);
                        WhenInvTemplateChange(false);
                    }
                }
                else // if it is fail, 
                {
                    Debug.Log("Fail to Transfer Equipment Card!"); // Have to display the message to player.
                }
                break;
        }
    }

    public bool ReceiveCard(CardData carddata) // Receive a carddata from other list, check if it is ok to receive.
    {
        if (InvCardList.Exists(x => x == carddata)) // Find if there is same carddata.
                                                     // If so, only need to add carddata to list.
        {
            if (true) // if the holdnum of that card reach limit or not ( not neccesary feature )
            {
                InvCardList.Add(carddata);
                WhenInvListChange(true);
                return true;
            }
        }
        else // if there are not same card in deck, need to add carddata to list and create new template.
        {
            InvCardList.Add(carddata);
            SpawnOneTemplateAndGetCom();
            WhenInvTemplateChange(true);
            return true;
        }
    }

    private void WhenInvListChange(bool AddorRemove) // Call by whenever the element in Inventory list is added or removed, but not effect template.
    {
        if ( AddorRemove ) //Add
        {
            SortListinAssetinInvList();
            DisplayAllCardtoInventory();
            UpdatePageNum();
        }
        else //Remove
        {
            DisplayAllCardtoInventory();
            UpdatePageNum();
        }
    }

    private void WhenInvTemplateChange(bool AddorRemove)// Call by whenever the element in Inventory list is added or removed, and cause the template change.
    {
        if (AddorRemove) //Add
        {
            SortListinAssetinInvList();
            DisplayAllCardtoInventory();
            UpdatePageNum();
        }
        else //Remove
        {
            UpdatePageNum();
        }
    }
}
