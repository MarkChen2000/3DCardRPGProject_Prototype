using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSystemAndUIController : MonoBehaviour
{
    private PlayerStatusController PlayerStstusCon;
    private InventoryController InventoryCon;
    private StatusUIManager _StatusUIManager;

    public GameObject PackCardTemp;

    private Transform Trans_ShopUIBGPanel;
    private Transform Trans_PackCardGridGroup;
    private TMP_Text TMP_AmountofMoney;
    private TMP_Text TMP_CostofPack;
    private TMP_Text TMP_ShopKeeperName;
    private TMP_Text TMP_PackName;

    private List<Transform> Trans_PackCardSlotTransList = new List<Transform>();

    private List<CardData> Current_PackCardPoolList = new List<CardData>();

    public int AmountofCardinOnePack = 5;
    private int _CostofOnePack ;

    private bool Did_Buy = false;


    private void Awake()
    {
        PlayerStstusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
        InventoryCon = GameObject.Find("InventoryAndUIManager").GetComponent<InventoryController>();
        _StatusUIManager = GameObject.Find("BattleUI").GetComponent<StatusUIManager>();

        Trans_ShopUIBGPanel = transform.GetChild(0).GetChild(2);
        Trans_PackCardGridGroup = Trans_ShopUIBGPanel.GetChild(0).GetChild(0);
        TMP_AmountofMoney = Trans_ShopUIBGPanel.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        TMP_CostofPack = Trans_ShopUIBGPanel.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        TMP_ShopKeeperName = Trans_ShopUIBGPanel.GetChild(3).GetChild(0).GetComponent<TMP_Text>();
        TMP_PackName = Trans_ShopUIBGPanel.GetChild(4).GetChild(0).GetComponent<TMP_Text>();

        GetPackCardSlotTrans();
    }

    private void GetPackCardSlotTrans()
    {
        for (int i = 0; i < AmountofCardinOnePack; i++)
        {
            Trans_PackCardSlotTransList.Add(Trans_PackCardGridGroup.GetChild(i));
        }
    }

    public void EnterShopSystem(TransactionTemplate transaction_tem)
    {
        Did_Buy = false;
        GetPackCardPoolfromTem(transaction_tem.PackCardPoolList);
        _CostofOnePack = transaction_tem.CostofOnePack;
        InitializeTexts(transaction_tem);
    }

    public void LeaveShopSystem()
    {
        if ( Did_Buy )
        {
            DestroyAllPackCardTemplates();
        }
    }

    private void InitializeTexts(TransactionTemplate transaction_tem)
    {
        TMP_ShopKeeperName.text = transaction_tem.ShopKeeperName;
        TMP_PackName.text = transaction_tem.PackName;
        UpdateMoneyText();
        TMP_CostofPack.text = "Cost : " + _CostofOnePack;
    }

    private void GetPackCardPoolfromTem(CardList cardpool)
    {
        Current_PackCardPoolList.Clear();
        foreach (CardData card in cardpool._CardList)
        {
            Current_PackCardPoolList.Add(card);
        }
    }

    private CardData GachaOneCardfromCurrentPool()
    {
        int index = Random.Range(0, Current_PackCardPoolList.Count);
        return Current_PackCardPoolList[index];
    }

    private List<CardData> Current_BuyingCards = new List<CardData>();

    public void BuyOnePackFromCurrentPool()
    {
        if ( PlayerStstusCon.Money < _CostofOnePack )
        {
            Debug.Log("You don't have enough money!");
            return;
        }

        PlayerStstusCon.Money -= _CostofOnePack;
        UpdateMoneyText();

        Current_BuyingCards.Clear();
        for ( int i = 0; i < AmountofCardinOnePack; i++) 
        {
            CardData card = GachaOneCardfromCurrentPool();
            Current_BuyingCards.Add( card );
            InventoryCon.ReceiveCard( card ) ; // transfer to inventory
        }

        SpawnAndDisplayPackCardTemplate();
        Did_Buy = true;
    }

    private void SpawnAndDisplayPackCardTemplate()
    {
        if ( Did_Buy ) // if already bought, don't need to instantiate new template.
        {
            for (int i = 0; i < AmountofCardinOnePack; i++)
            {
                PackCard_DataLoaderAndDisplay cardtemp = Trans_PackCardSlotTransList[i].GetChild(0).GetComponent<PackCard_DataLoaderAndDisplay>();
                cardtemp.DisplaytoTemplate(Current_BuyingCards[i]);
            }
        }
        else
        {
            for (int i = 0; i < AmountofCardinOnePack; i++)
            {
                PackCard_DataLoaderAndDisplay cardtemp = Instantiate(PackCardTemp, Trans_PackCardSlotTransList[i]).GetComponent<PackCard_DataLoaderAndDisplay>();
                cardtemp.DisplaytoTemplate(Current_BuyingCards[i]);
            }
        }
    }

    private void DestroyAllPackCardTemplates()
    {
        for (int i = 0; i < AmountofCardinOnePack; i++)
        {
            Destroy(Trans_PackCardSlotTransList[i].GetChild(0).gameObject);
        }
    }
    
    private void UpdateMoneyText()
    {
        TMP_AmountofMoney.text = " Money : " + PlayerStstusCon.Money;
        _StatusUIManager.UpdateOneStatusDisplay(StatusType.Money);
    }

}
