using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class CardBattleController : MonoBehaviour
{
    public GameObject card;
    private int maxCardAtOnce = 5;
    //private List<BattleCard_LoaderAndDisplay> displayTemplateComponent = new List<BattleCard_LoaderAndDisplay>();
    //private System.Random rnd = new System.Random();
    //private PlayerStatus playerStatus;
    private Transform Trans_HandCardsPanel;
    private Transform Trans_BattleCardTemplates;

    private StatusUIManager _StstusUIManager;
    private PlayerStatusController PlayerStatusCon;
    private CardDeckController CardDeckCon;

    [Space]
    public CardList tempBattleCardList;
    public List<CardData> BattleCardList = new List<CardData>(); // CardList that store at this script

    public List<CardData> LastBattleCardList = new List<CardData>();

    private void Awake()
    {

        PlayerStatusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
        CardDeckCon = GameObject.Find("InventoryAndUIManager").GetComponent<CardDeckController>();
        _StstusUIManager = GetComponent<StatusUIManager>();

        Trans_HandCardsPanel = transform.GetChild(0).GetChild(1);
        Trans_BattleCardTemplates = Trans_HandCardsPanel.GetChild(0);

        /*for (int i = 0; i < maxCardAtOnce; i++)
        {
            displayTemplateComponent.Add(Trans_HandCardsPanel.GetChild(0).GetChild(i).GetComponent<BattleCard_LoaderAndDisplay>());
            this.displayTemplateComponent[i].transform.SetSiblingIndex(i);
        }*/
    }

    private void Start()
    {
        SwitchHandCardDisplay(false); // turn the hand card off at first (defaultly will in the village).
    }

    public void EnterCombatInitialize() 
    {
        SwitchHandCardDisplay(true);

        for (int i = 0; i < Trans_BattleCardTemplates.childCount ; i++) // Destory all the remaining card teamplate in hand.
        {
            Destroy(Trans_BattleCardTemplates.GetChild(i).gameObject);
        } 

        BattleCardList.Clear();

        for (int i = 0; i < CardDeckCon.DeckCardList.Count; i++) // copy the value from Deck.
        {
            BattleCardList.Add(CardDeckCon.DeckCardList[i]);
        }

        for (int i = 0; i < maxCardAtOnce; i++) // first draw
        {
            DrawCard();
        }
    }

    public void SwitchHandCardDisplay(bool b)
    {
        if (b)
        {
            Trans_HandCardsPanel.gameObject.SetActive(true);
        }
        else
        {
            Trans_HandCardsPanel.gameObject.SetActive(false);
        }
    }

    // Doesn't need this part, we can spawn card in DrawCard function and no need to display the card at first.
    /*public void DisplayCard()
    {
        if (this.LastBattleCardList.Count > 0)
        {
            for (int i = 0; i < this.LastBattleCardList.Count; i++)
            {
                displayTemplateComponent[i].DisplaytoTemplate(this.LastBattleCardList[i]);
            }
        }
        else
        {
            if (this.BattleCardList.Count < this.maxCardAtOnce)
            {
                this.maxCardAtOnce = this.BattleCardList.Count;
            }
            for (int i = 0; i < maxCardAtOnce; i++)
            {
                int temp = rnd.Next(BattleCardList.Count - i);
                displayTemplateComponent[i].DisplaytoTemplate(BattleCardList[temp]); // the GetData function will be replace by DisplaytoTemplates function, pls check the card_loadanddisplay script.
                BattleCardList.RemoveAt(temp);
                this.LastBattleCardList.Add(BattleCardList[temp]);
            }
            this.maxCardAtOnce = 5;
        }
    }*/

    public void updateStatus(CardData card)
    {
        Dictionary<string, int> statusDict = PlayerStatusCon.GetStatus();
        PlayerStatusCon.UpdateStatus("currentMana", -card.CardCost);
        _StstusUIManager.UpdateOneStatusDisplay(StatusType.Mana);
    }

    public void DrawCard()
    {
        Debug.Log("Draw!");
        if (this.BattleCardList.Count > 0)
        {
            // Instantiate a card teamplate and get the cardloader component on it.
            BattleCard_LoaderAndDisplay cardtemp = Instantiate(card, Trans_BattleCardTemplates).GetComponent<BattleCard_LoaderAndDisplay>();

            int index = Random.Range(0, BattleCardList.Count); // have to disaable using System on the top to use this random function, I don't know why.
            cardtemp.DisplaytoTemplate(BattleCardList[index]);
            BattleCardList.RemoveAt(index);

            // Sorry have to comment all the part, but it will some how cause a warning sign, but i am not sure why.
            /*int temp = rnd.Next(BattleCardList.Count);
            GameObject newCard = Instantiate(card, GameObject.Find("BattleCardTemplates").transform);
            newCard.gameObject.GetComponent<BattleCard_LoaderAndDisplay>().DisplaytoTemplate(BattleCardList[temp]);
            BattleCardList.RemoveAt(temp);
            this.LastBattleCardList.Add(BattleCardList[temp]);*/
        }
        else Debug.Log("There is no card in the remaining deck!");
    }

}
