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
    private Transform Trans_HandCardsPanel;

    private StatusUIManager _StstusUIManager;
    private PlayerStatusController PlayerStatusCon;

    [Space]
    public CardList tempBattleCardList;
    public List<CardData> BattleCardList = new List<CardData>(); // CardList that store at this script

    private void Awake()
    {
        SaveandLoadBattleCardList(true); // Load
        if (tempBattleCardList == null) tempBattleCardList = Resources.Load<CardList>("CardLists_SO/Testing_BattleCardList");
        // Load initial BattleCardList asset first, this may be replaced by Save and Load System before build!
        InitializeLoadinData();

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

    private void InitializeLoadinData()
    {
        // InvCardList = _InvCardListAsset._CardList; // only copy it's reference.

        BattleCardList.Clear();
        for (int i = 0; i < tempBattleCardList._CardList.Count; i++) // copy the value from SO asset list.
        {
            BattleCardList.Add(tempBattleCardList._CardList[i]);
        }
    }

    private void SaveandLoadBattleCardList(bool SorL)
    {
        // save and load system.
        if (SorL)
        { }
        else
        { }
    }


    public void Activate(bool b)
    {
        if (b)
        {
            Trans_HandCardsPanel.gameObject.SetActive(true);
            /*if (this.tempBattleCardList == null) // not sure what is this code's function. - Chen
            {
                tempBattleCardList = Instantiate(Resources.Load<CardList>("CardLists_SO/Testing_BattleCardList"));
            }*/
            DisplayCard();
        }
        else
        {
            Trans_HandCardsPanel.gameObject.SetActive(false);
        }
    }

    public void DisplayCard()
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
        if (this.BattleCardList.Count > 0)
        {
            int temp = rnd.Next(BattleCardList.Count);
            GameObject newCard = Instantiate(card, GameObject.Find("BattleCardTemplates").transform);
            newCard.gameObject.GetComponent<BattleCard_LoaderAndDisplay>().DisplaytoTemplate(BattleCardList[temp]);
            BattleCardList.RemoveAt(temp);
        }
    }

}
