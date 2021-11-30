using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class CradBattleUIController : MonoBehaviour
{
    private CardList battleCardList;
    private const int maxCardAtOnce = 5;
    private List<BattleCard_LoaderAndDisplay> displayTemplateComponent = new List<BattleCard_LoaderAndDisplay>();
    private System.Random rnd = new System.Random();
    private PlayerStatus playerStatus;
    private CardList tempBattleCardList;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < maxCardAtOnce; i++)
        {
            //displayTemplateComponent.Add(template.GetChild(i).GetComponent<Card_DataLoaderAndDisplay>());
            displayTemplateComponent.Add(this.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<BattleCard_LoaderAndDisplay>());
        }

        battleCardList = Resources.Load<CardList>("CardLists_SO/Testing_BattleCardList");

        if (tempBattleCardList == null)
        {
            tempBattleCardList = Resources.Load<CardList>("CardLists_SO/Temp_BattleCardList");
            tempBattleCardList._CardList.RemoveRange(0, tempBattleCardList._CardList.Count);
            foreach (var item in battleCardList._CardList)
            {
                tempBattleCardList._CardList.Add(item);
            }
        }

        DisplayCard();

        playerStatus = Resources.Load<PlayerStatus>("Player/MainPlayer");

        DisplayStatus();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayCard()
    {
        for (int i = 0; i < maxCardAtOnce; i++)
        {
            //BattleCard_LoaderAndDisplay cardloader = displayTemplateComponent[i];
            int temp = rnd.Next(tempBattleCardList.getLength() - i);
            displayTemplateComponent[i].DisplaytoTemplate(tempBattleCardList._CardList[temp]); // the GetData function will be replace by DisplaytoTemplates function, pls check the card_loadanddisplay script.
            //this.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<BattleCard_LoaderAndDisplay>()._CardData = tempBattleCardList._CardList[temp];
            tempBattleCardList._CardList.RemoveAt(temp);
        }

    }

    public void DisplayStatus()
    {
        this.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "LV: " + playerStatus.lv;
        this.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = "Health: 100/" + playerStatus.max_hp;
        this.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<TMP_Text>().text = "Mana: 10/" + playerStatus.max_mp;
        this.transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<TMP_Text>().text = "Attack: " + playerStatus.attack;
        this.transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<TMP_Text>().text = "Defense: " + playerStatus.defense;
    }

    public void updateStatus(CardData card)
    {
        this.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<TMP_Text>().text = "Mana: " + (10 - card.CardCost) + "/" + playerStatus.max_mp;
    }
}
