using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class CardBattleController : MonoBehaviour
{
    public GameObject card;
    private int maxCardAtOnce = 5;
    private List<BattleCard_LoaderAndDisplay> displayTemplateComponent = new List<BattleCard_LoaderAndDisplay>();
    private System.Random rnd = new System.Random();
    //private PlayerStatus playerStatus;
    private CardList tempBattleCardList;
    private Transform Trans_HandCardsPanel;

    private StatusUIManager _StstusUIManager;
    private PlayerStatusController PlayerStatusCon;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Trans_HandCardsPanel = transform.GetChild(0).GetChild(1);
        Trans_HandCardsPanel.gameObject.SetActive(false);
        for (int i = 0; i < maxCardAtOnce; i++)
        {
            displayTemplateComponent.Add(Trans_HandCardsPanel.GetChild(0).GetChild(i).GetComponent<BattleCard_LoaderAndDisplay>());
        }

        _StstusUIManager = GetComponent<StatusUIManager>();
        PlayerStatusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
    }

    public void Activate(bool b)
    {
        if (b)
        {
            Trans_HandCardsPanel.gameObject.SetActive(true);
            if (this.tempBattleCardList == null)
            {
                tempBattleCardList = Instantiate(Resources.Load<CardList>("CardLists_SO/Testing_BattleCardList"));
            }
            DisplayCard();
        }
        else
        {
            Trans_HandCardsPanel.gameObject.SetActive(false);
        }
    }

    public void DisplayCard()
    {
        if (this.tempBattleCardList.getLength() < this.maxCardAtOnce)
        {
            this.maxCardAtOnce = this.tempBattleCardList.getLength();
        }
        for (int i = 0; i < maxCardAtOnce; i++)
        {
            int temp = rnd.Next(tempBattleCardList.getLength() - i);
            displayTemplateComponent[i].DisplaytoTemplate(tempBattleCardList._CardList[temp]); // the GetData function will be replace by DisplaytoTemplates function, pls check the card_loadanddisplay script.
            tempBattleCardList._CardList.RemoveAt(temp);
        }
        this.maxCardAtOnce = 5;
    }

    public void updateStatus(CardData card)
    {
        Dictionary<string, int> statusDict = PlayerStatusCon.GetStatus(); 
        PlayerStatusCon.UpdateStatus("currentMana", -card.CardCost);
        _StstusUIManager.UpdateOneStatusDisplay(StatusType.Mana);
    }

    public void DrawCard()
    {
        if (this.tempBattleCardList.getLength() > 0)
        {
            int temp = rnd.Next(tempBattleCardList.getLength());
            GameObject newCard = Instantiate(card, GameObject.Find("BattleCardTemplates").transform);
            newCard.gameObject.GetComponent<BattleCard_LoaderAndDisplay>().DisplaytoTemplate(tempBattleCardList._CardList[temp]);
            tempBattleCardList._CardList.RemoveAt(temp);
        }
    }
}
