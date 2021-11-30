using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryController : MonoBehaviour
{
    private CardDeckController CardDeck_Con;
    private EquipmentSlotController EquipSlot_Con;

    private Transform Trans_InventoryCanvas;
    private Canvas Canvas_Inventory;
    private TMP_Text TMP_PageNum;

    public GameObject CardTemplatePrefab; // the template that display on the panel
    [Tooltip("InventoryController_CardListAsset")]
    public CardList IC_CardListAsset; // InventoryController_CardList, the CardList-type asset
    private List<CardData> Inventory_CardList = new List<CardData>(); // CardList that store at this script

    private Transform Trans_InvCardsGridGroup; // locattion of the InvCardGridGroup
    private List<Card_DataLoaderAndDisplay> TemplateComponent = new List<Card_DataLoaderAndDisplay>(); // Each Template Component which is Display 

    [Space]
    public int TemplateNum = 10; // the amount of card tmeplate can display in one page
    private int CurrentPage = 1;
    private int MaxPage = 1;

    private void Awake()
    {
        //GetComponentofTemplates();

        EquipSlot_Con = GetComponent<EquipmentSlotController>();
        CardDeck_Con = GetComponent<CardDeckController>();
        Trans_InventoryCanvas = transform.GetChild(0).GetComponent<Transform>();
        Canvas_Inventory = Trans_InventoryCanvas.GetComponent<Canvas>();
        Trans_InvCardsGridGroup = Trans_InventoryCanvas.GetChild(1).GetChild(0).transform;
        TMP_PageNum = Trans_InventoryCanvas.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Text>();

        LoadDatafromListAsset();
    }

    private void Start()
    {
        SpawnAllTemplateAndGetCom(); // Spawn template and get component.
        DisplayAllCardtoInventory(); // Renew Display the Current page of Cards.
        UpdatePageNum();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // TurnOn or Off the Inventory UI.
        {
            if (Canvas_Inventory.enabled == true)
            {
                Canvas_Inventory.enabled = false;
            }
            else
            {
                Canvas_Inventory.enabled = true;
            }
        }
    }

    private void LoadDatafromListAsset()
    {
        if (IC_CardListAsset == null)
        {
            IC_CardListAsset = Resources.Load<CardList>("CardLists_SO/Testing_InventoryCardList"); // Because only CardListAsset can use Sort function, so it has to be load in as a asset.
        }
        // Load CardList-type of data, which have to put into "Resources" file, then get the CardData-type List in it.

        Inventory_CardList = IC_CardListAsset._CardList;

        IC_CardListAsset.RemoveAllEmptyElement(); // All the load-in asset need to be formalize to edit.
    }

    private void UpdatePageNum()
    {
        if (Inventory_CardList == null)
        {
            TMP_PageNum.text = "1/X";
            return; // if there is no list data, return.
        }

        if (Inventory_CardList.Count == 0) MaxPage = 1; // if there is a list but dont have any data in it, Set MaxPage = 1.
        else MaxPage = (int)Mathf.Ceil((float)IC_CardListAsset.GetDiffCardNum() / (float)TemplateNum); // 無條件進位 maxpage

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
        if (IC_CardListAsset.GetDiffCardNum() == 0) return;
        for (int i = 0; i < IC_CardListAsset.GetDiffCardNum(); i++)
        {
            SpawnOneTemplateAndGetCom();
        }
    }

    private void SpawnOneTemplateAndGetCom()
    {
        GameObject gb = Instantiate(CardTemplatePrefab, Trans_InvCardsGridGroup);
        TemplateComponent.Add(gb.GetComponent<Card_DataLoaderAndDisplay>());
    }

    private void DisplayAllCardtoInventory()
    {
        if (IC_CardListAsset.GetDiffCardNum() == 0) return;

        int cardnumcount = 1;
        for (int i = 0, k = 0; i < IC_CardListAsset.GetDiffCardNum() ;k++) // i as template count, k as the count of elements in list.
        {
            if ( i != IC_CardListAsset.GetDiffCardNum()-1 )
            {
                if (Inventory_CardList[k] == Inventory_CardList[k + 1]) // if two near element is same, then counter +1.
                {
                    cardnumcount++;
                }
                else 
                {
                    TemplateComponent[i].HoldNum = cardnumcount;
                    TemplateComponent[i].DisplaytoTemplate(Inventory_CardList[k]);
                    i++;
                    cardnumcount = 1;
                }
            }
            else // if it is the last different card in the list.
            {
                TemplateComponent[i].HoldNum = Inventory_CardList.Count - k ;
                TemplateComponent[i].DisplaytoTemplate(Inventory_CardList[k]);
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

    public void TryTransferCard(Card_DataLoaderAndDisplay card_template) 
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
                        Inventory_CardList.Remove(card_template._CardData);
                        WhenInvListChange(false);
                    }
                    else if (card_template.HoldNum == 1)
                    {
                        int tem_pos;
                        Inventory_CardList.Remove(card_template._CardData);
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
                        Inventory_CardList.Remove(card_template._CardData);
                        WhenInvListChange(false);
                    }
                    else if (card_template.HoldNum == 1)
                    {
                        int tem_pos;
                        Inventory_CardList.Remove(card_template._CardData);
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
        if (Inventory_CardList.Exists(x => x == carddata)) // Find if there is same carddata.
                                                     // If so, only need to add carddata to list.
        {
            if (true) // if the holdnum of that card reach limit or not ( not neccesary feature )
            {
                Inventory_CardList.Add(carddata);
                WhenInvListChange(true);
                return true;
            }
        }
        else // if there are not same card in deck, need to add carddata to list and create new template.
        {
            Inventory_CardList.Add(carddata);
            SpawnOneTemplateAndGetCom();
            WhenInvTemplateChange(true);
            return true;
        }
    }

    private void WhenInvListChange(bool AddorRemove) // Call by whenever the element in Inventory list is added or removed, but not effect template.
    {
        if ( AddorRemove ) //Add
        {
            IC_CardListAsset.SortListinAsset();
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
            IC_CardListAsset.SortListinAsset();
            DisplayAllCardtoInventory();
            UpdatePageNum();
        }
        else //Remove
        {
            UpdatePageNum();
        }
    }

    // This is Original Method to Display Templates by particular page down below.

    /*private void GetComponentofTemplates()
    {
        Trans_BPCardDisplayBGPanel = transform.GetChild(0).GetChild(0) ;

        for (int i = 0; i < TemplateNum ; i++) // Get each CardTemplate
        {
            DisplayTemplateComponent.Add(Trans_BPCardDisplayBGPanel.GetChild(i).GetComponent<Card_DataLoaderAndDisplay>()); // Currently the List is empty, so use "add" instead of "=".
        }
    }*/

    /*public void DisplayAllCardtoInventory(int page) // put in which page should display
    {
        int displaytime = page*TemplateNum ;

        for (int i = (page-1)*TemplateNum , j = 0 ; i < displaytime ; i++, j++) 
        // "i" mean the count of the elements in the list. "j" mean the count of the template in a page.  
        {
            Card_DataLoaderAndDisplay cardloader;
            cardloader = DisplayTemplateComponent[j];

            if ( IC_CardListAsset.GetDiffCardNum() > i ) // if the list is shortter then displaynum, then stop try to get element, there is no element in the list.
            {

                int cardnumcount = 1;

                for (int k = i; true ; k++)
                {
                    if ( Inventory_CardList[k]==Inventory_CardList[k+1] )
                    {
                        cardnumcount++;
                    }
                    else
                    {
                        cardloader.DisplaytoTemplates(Inventory_CardList[i]);
                        cardloader.HoldNum = cardnumcount;
                        i = k + 1;
                        break;
                    }
                }
            }
            else 
            {
                cardloader.DisplaytoTemplates(null);
            }
        }
    }*/
}
