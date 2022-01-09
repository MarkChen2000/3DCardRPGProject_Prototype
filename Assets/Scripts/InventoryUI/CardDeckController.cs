using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDeckController : MonoBehaviour
{
    private InventoryController Inv_Con;

    public GameObject DeckCardTemplatePrefab;
    private Transform Trans_DeckCardGridGroup;
    private Transform Trans_InventoryPanelBG;
    private TMP_Text TMP_DeckCapacityNumText;
    PopupInfoUIManager _PopupUIManager;

    private List<DeckCard_DataLoaderAndDisplay> TemplateComponent = new List<DeckCard_DataLoaderAndDisplay>();

    public int DeckHoldLimit = 30;

    [Space]
    [Tooltip("If you leave this reference as null, it will automatical reference the gameplay data.\nIn other word, you can reference testing data if you want to, but eventually need to leave this as null before building.")]
    public CardList _CardDeckAsset; // CardList-type asset.
    public List<CardData> DeckCardList = new List<CardData>();

    private void Awake()
    {
        _PopupUIManager = _PopupUIManager = GameObject.Find("InfoUIManager").GetComponent<PopupInfoUIManager>();
        Inv_Con = GetComponent<InventoryController>();
        Trans_InventoryPanelBG = transform.GetChild(0).GetChild(0);
        Trans_DeckCardGridGroup = Trans_InventoryPanelBG.GetChild(0).GetChild(0);
        TMP_DeckCapacityNumText = Trans_InventoryPanelBG.GetChild(0).GetChild(1).GetChild(1).GetComponent<TMP_Text>();

        if (_CardDeckAsset == null) _CardDeckAsset = Resources.Load<CardList>("CardLists_SO/Player_BattleCardList");
        // Load initial DeckCardList asset first, this may be replaced by Save and Load System before build!
    }

    private void Start()
    {
        InitializeLoadinData();
        SpawnAllTemplateandGetCom();
        DisplayAllCardtoDeck();
        UpdateCardNum();
    }

    private void InitializeLoadinData()
    {
        //DeckCardList = _CardDeckAsset._CardList; // only copy by reference

        DeckCardList.Clear();
        for (int i = 0; i < _CardDeckAsset._CardList.Count; i++) // copy the value.
        {
            DeckCardList.Add(_CardDeckAsset._CardList[i]);
        }
    }

    public void SaveandLoadCardDeck(bool SorL)
    {
        // save and load system.
        if (SorL )
        { }
        else 
        { }
    }

    private void SortDeckList() // Sort the cards in the list.
    {
        if (DeckCardList.Count == 0)
        {
            Debug.Log("Didnt have card inside the CardLIst !");
        }
        else
        {
            // Because Carddata-type data is not the regular type like int or string that can be sorted defaulty.
            // Have to use "IComparble" to compare the data (CardCost) of elements in list, with self-made sort function.
            DeckCardList.Sort(); // Sort by the Cost of Card.
        }
    }

    private int GetDiffCardNuminDeck() // Get how many DIFFERENT Card in the list;
    {
        if (DeckCardList.Count == 0) return 0;

        SortDeckList();
        int differnrtcount = 1;
        for (int i = 0; i < DeckCardList.Count - 1; i++)
        {
            if (DeckCardList[i] != DeckCardList[i + 1])
            {
                differnrtcount++;
            }
        }
        return differnrtcount;
    }

    private void SpawnAllTemplateandGetCom()
    {
        for (int i = 0; i < GetDiffCardNuminDeck() ; i++)
        {
            SpawnOneTemplateandGetCom();
        }
    }
    private void SpawnOneTemplateandGetCom() // For When a new card add in deck.
    {
        GameObject template = Instantiate(DeckCardTemplatePrefab, Trans_DeckCardGridGroup); // Spawn a template prefab as Deck panel child.
        TemplateComponent.Add(template.GetComponent<DeckCard_DataLoaderAndDisplay>());
    }

    private void DisplayAllCardtoDeck()
    {
        if (GetDiffCardNuminDeck() == 0) return;

        int cardnumcount = 1;
        for (int i = 0, k = 0; i < GetDiffCardNuminDeck(); k++) // i as template count, k as the count of elements in list.
        {
            //Debug.Log("A"); //for bug fixing.
            if (i != GetDiffCardNuminDeck() - 1)
            {
                //Debug.Log("B"); //for bug fixing.
                if (DeckCardList[k] == DeckCardList[k + 1]) // if two near element is same, then counter +1.
                {
                    //Debug.Log("C"); //for bug fixing.
                    cardnumcount++;
                }
                else
                {
                    //Debug.Log("D"); //for bug fixing.
                    TemplateComponent[i].HoldNum = cardnumcount;
                    TemplateComponent[i].DisplaytoTemplate(DeckCardList[k]);
                    i++;
                    cardnumcount = 1;
                }
            }
            else // if it is the last different card in the list.
            { 
                //Debug.Log("E"+i+k); //for bug fixing.
                TemplateComponent[i].HoldNum = DeckCardList.Count - k;
                //Debug.Log("F"); //for bug fixing.
                TemplateComponent[i].DisplaytoTemplate(DeckCardList[k]);
                i++;
            }
        }

    }

    private void UpdateCardNum()
    {
        int currentcardnum = DeckCardList.Count;
        TMP_DeckCapacityNumText.text = currentcardnum + "/" + DeckHoldLimit;
    }

    public void TryTransferCardtoInv(BasicCard_DataLoaderAndDisplay card_template)
    // Try to transfer card from this list to another list,
    // need to get the component of cardt template, and get the carddata and holdnum from it.
    {
        if (Inv_Con.ReceiveCard(card_template._CardData)) // if it is success to transfer card, check Inv_Con if it is ok to transfer.
        {
            if (card_template.HoldNum > 1) // it means that doesn't need to remove the template, but remove one of the carddata from list. 
            {
                DeckCardList.Remove(card_template._CardData);
                WhenDeckListChange(true);
            }
            else if (card_template.HoldNum == 1)
            {
                int tem_pos;
                DeckCardList.Remove(card_template._CardData);
                tem_pos = card_template.transform.GetSiblingIndex(); // Get the position under parent in the hierarchy, which is also equal the component index in the list.
                Destroy(card_template.gameObject);
                TemplateComponent.RemoveAt(tem_pos); // and remove the component at that index.
                WhenDeckTemplateChange(false);
            }
        }
        else // if it is fail, 
        {
            //Debug.Log("Fail to Transfer from Deck to Inv!"); // Have to display the message to player.
            return;
        }
    }

    public bool ReceiveCard(CardData carddata) // Receive a carddata from other list, check if it is ok to receive.
    {
        switch (carddata._CardType) // check what type of this card.
        {
            case CardType.Spells:
                if (DeckHoldLimit > DeckCardList.Count) // If there still have space for card.
                {
                    if (DeckCardList.Exists(x => x == carddata)) // Find if there is same carddata.
                                                                              // If so, only need to add carddata to list.
                    {
                        if (true) // if the holdnum of that card reach limit or not ( not neccesary feature )
                        {
                            DeckCardList.Add(carddata);
                            WhenDeckListChange(true);
                            return true;
                        }
                    }
                    else // if there are not same card in deck, need to add carddata to list and create new template.
                    {
                        DeckCardList.Add(carddata);
                        SpawnOneTemplateandGetCom();
                        WhenDeckTemplateChange(true);
                        return true;
                    }
                }
                else
                {
                    _PopupUIManager.SpawnPopupInfoUI("Deck Doesn't Have Enough Room!");
                    //Debug.Log("Deck Doesn't Have Enough Room!");
                    return false;
                }
            case CardType.Equipment:
                Debug.Log("That is not Spells!");
                return false;
        }
        return false;
    }

    private void WhenDeckListChange(bool AddorRemove) // Call by whenever the element in Deck list is added or removed, but not effect template.
    {
        if (AddorRemove) //Add
        {
            SortDeckList();
        }
        else //Remove
        {
        }
        DisplayAllCardtoDeck();
        UpdateCardNum();
    }

    private void WhenDeckTemplateChange(bool AddorRemove)// Call by whenever the element in Deck list is added or removed, and cause the template change.
    {
        if (AddorRemove) //Add
        {
            SortDeckList();
            DisplayAllCardtoDeck();
        }
        else //Remove
        {
        }
        UpdateCardNum();
    }
}
