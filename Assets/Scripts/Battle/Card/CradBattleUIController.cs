using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class CradBattleUIController : MonoBehaviour
{
    public GameObject card;
    private int maxCardAtOnce = 5;
    private List<BattleCard_LoaderAndDisplay> displayTemplateComponent = new List<BattleCard_LoaderAndDisplay>();
    private System.Random rnd = new System.Random();
    //private PlayerStatus playerStatus;
    private CardList tempBattleCardList;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        for (int i = 0; i < maxCardAtOnce; i++)
        {
            displayTemplateComponent.Add(this.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<BattleCard_LoaderAndDisplay>());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate(bool b)
    {
        if (b)
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            if (this.tempBattleCardList == null)
            {
                tempBattleCardList = Instantiate(Resources.Load<CardList>("CardLists_SO/Testing_BattleCardList"));
            }

            DisplayCard();
            DisplayStatus();
        }
        else
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
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

    public void DisplayStatus()
    {
        Dictionary<string, int> statusDict = GameObject.Find("InventoryManager").GetComponent<EquipmentSlotController>().GetStatus();
        this.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "LV: " + statusDict["LV"];
        this.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = "HP: " + statusDict["CurrentHP"] + "/" + statusDict["MaxHP"];
        this.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<TMP_Text>().text = "MP: " + statusDict["CurrentMP"] + "/" + statusDict["MaxMP"];
        this.transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<TMP_Text>().text = "Atk: " + statusDict["Attack"];
        this.transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<TMP_Text>().text = "Def: " + statusDict["Defense"];
    }

    public void updateStatus(CardData card)
    {
        Dictionary<string, int> statusDict = GameObject.Find("InventoryManager").GetComponent<EquipmentSlotController>().GetStatus();
        this.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<TMP_Text>().text = "MP: " + (10 - card.CardCost) + "/" + statusDict["MaxMP"];
    }

    public void DrawCard()
    {
        if (this.tempBattleCardList.getLength() > 0)
        {
            int temp = rnd.Next(tempBattleCardList.getLength());
            GameObject newCard = Instantiate(card, GameObject.Find("CardTemplate").transform);
            newCard.gameObject.GetComponent<BattleCard_LoaderAndDisplay>().DisplaytoTemplate(tempBattleCardList._CardList[temp]);
            tempBattleCardList._CardList.RemoveAt(temp);
        }
    }
}
