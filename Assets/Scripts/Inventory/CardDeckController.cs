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
    private TMP_Text TMP_DeckCapacityNumText;

    [InspectorName("CardDeckController_CardListAsset")]
    public CardList CDC_CardListAsset; // CardList-type asset.
    [HideInInspector]
    public List<CardData> CDC_CardDeck = new List<CardData>() ;
    private List<DeckCard_DataLoaderAndDisplay> TemplateComponent = new List<DeckCard_DataLoaderAndDisplay>();

    [Space]
    public int DeckHoldLimit = 30;

    private void Awake()
    {
        Inv_Con = GetComponent<InventoryController>();
        Trans_DeckCardGridGroup = transform.GetChild(0).GetChild(0).GetChild(0);
        TMP_DeckCapacityNumText = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_Text>();

        LoadDatafromListAsset();
    }

    private void Start()
    {
        SpawnAllTemplateandGetCom();
        DisplayAllCardtoDeck();
        UpdateCardNum();
    }

    private void LoadDatafromListAsset()
    {
        if (CDC_CardListAsset == null)
        {
            CDC_CardListAsset = Resources.Load<CardList>("CardLists_SO/Testing_BattleCardList");
        }
        CDC_CardDeck = CDC_CardListAsset._CardList;

        CDC_CardListAsset.RemoveAllEmptyElement();
    }

    private void SpawnAllTemplateandGetCom()
    {
        for (int i = 0; i < CDC_CardListAsset.GetDiffCardNum() ; i++)
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
        if (CDC_CardListAsset.GetDiffCardNum() == 0) return;

        int cardnumcount = 1;
        for (int i = 0, k = 0; i < CDC_CardListAsset.GetDiffCardNum(); k++) // i as template count, k as the count of elements in list.
        {
            //Debug.Log("A"); //for bug fixing.
            if (i != CDC_CardListAsset.GetDiffCardNum() - 1)
            {
                //Debug.Log("B"); //for bug fixing.
                if (CDC_CardDeck[k] == CDC_CardDeck[k + 1]) // if two near element is same, then counter +1.
                {
                    //Debug.Log("C"); //for bug fixing.
                    cardnumcount++;
                }
                else
                {
                    //Debug.Log("D"); //for bug fixing.
                    TemplateComponent[i].HoldNum = cardnumcount;
                    TemplateComponent[i].DisplaytoTemplate(CDC_CardDeck[k]);
                    i++;
                    cardnumcount = 1;
                }
            }
            else // if it is the last different card in the list.
            { 
                //Debug.Log("E"+i+k); //for bug fixing.
                TemplateComponent[i].HoldNum = CDC_CardDeck.Count - k;
                //Debug.Log("F"); //for bug fixing.
                TemplateComponent[i].DisplaytoTemplate(CDC_CardDeck[k]);
                i++;
            }
        }

    }

    private void UpdateCardNum()
    {
        int currentcardnum = CDC_CardDeck.Count;
        TMP_DeckCapacityNumText.text = currentcardnum + "\n/\n" + DeckHoldLimit;
    }

    public void TryTransferCardtoInv(DeckCard_DataLoaderAndDisplay card_template)
    // Try to transfer card from this list to another list,
    // need to get the component of cardt template, and get the carddata and holdnum from it.
    {
        if (Inv_Con.ReceiveCard(card_template._CardData)) // if it is success to transfer card, check Inv_Con if it is ok to transfer.
        {
            if (card_template.HoldNum > 1) // it means that doesn't need to remove the template, but remove one of the carddata from list. 
            {
                CDC_CardDeck.Remove(card_template._CardData);
                WhenDeckListChange(true);
            }
            else if (card_template.HoldNum == 1)
            {
                int tem_pos;
                CDC_CardDeck.Remove(card_template._CardData);
                tem_pos = card_template.transform.GetSiblingIndex(); // Get the position under parent in the hierarchy, which is also equal the component index in the list.
                Destroy(card_template.gameObject);
                TemplateComponent.RemoveAt(tem_pos); // and remove the component at that index.
                WhenDeckTemplateChange(false);
            }
        }
        else // if it is fail, 
        {
            Debug.Log("Fail to Transfer from Deck to Inv!"); // Have to display the message to player.
            return;
        }
    }

    public bool ReceiveCard(CardData carddata) // Receive a carddata from other list, check if it is ok to receive.
    {
        switch (carddata._CardType) // check what type of this card.
        {
            case CardType.Spells:
                if (DeckHoldLimit > CDC_CardDeck.Count) // If there still have space for card.
                {
                    if (CDC_CardDeck.Exists(x => x == carddata)) // Find if there is same carddata.
                                                                              // If so, only need to add carddata to list.
                    {
                        if (true) // if the holdnum of that card reach limit or not ( not neccesary feature )
                        {
                            CDC_CardDeck.Add(carddata);
                            WhenDeckListChange(true);
                            return true;
                        }
                    }
                    else // if there are not same card in deck, need to add carddata to list and create new template.
                    {
                        CDC_CardDeck.Add(carddata);
                        SpawnOneTemplateandGetCom();
                        WhenDeckTemplateChange(true);
                        return true;
                    }
                }
                else
                {
                    Debug.Log("Deck Doesn't Have Enough Room!");
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
            CDC_CardListAsset.SortListinAsset();
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
            CDC_CardListAsset.SortListinAsset();
            DisplayAllCardtoDeck();
        }
        else //Remove
        {
        }
        UpdateCardNum();
    }
}
