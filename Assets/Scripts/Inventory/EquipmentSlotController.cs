using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlotController : MonoBehaviour
{

    public GameObject EquipmentCardTemplate;
    private List<Transform> SlotPanelTransList = new List<Transform>();
    private List<EquipmentCard_DataLoaderAndDisplay> TemplateComponent = new List<EquipmentCard_DataLoaderAndDisplay>();

    private InventoryController Inv_Con;
    public EquipmentSlot ES_EquipmentSlotAsset;

    private Transform Trans_EquipmentSlotPanel;
    private Transform Trans_PlayerStatusPanel;
    private Transform Trans_EquipmentSlotGridGroup;
    private TMP_Text TMP_PlayerLV, TMP_PlayerHP, TMP_PlayerMP, TMP_PlayerAP, TMP_PlayerDP;

    [Space]
    /*
    public int Player_LV = 1; // Level    public int Player_BaseMaxHP = 100; //Health Point
    public int Player_BaseMaxHP = 100;
    private int Player_CurrentMaxHP;
    private int Player_CurrentHP;
    public int Player_BaseMaxMP = 10; //Mana Point
    private int Player_CurrentMaxMP; 
    private int Player_CurrentMP;
    public int Player_BaseAP = 10; //Attack Point
    private int Player_CurrentAP; 
    public int Player_BaseDP = 10; //Defend Point
    private int Player_CurrentDP; 

    private int EquipmentBonusHP = 0;
    private int EquipmentBonusMP = 0;
    private int EquipmentBonusAP = 0;
    private int EquipmentBonusDP = 0;
    */

    public int Player_LV; // Level    public int Player_BaseMaxHP = 100; //Health Point
    public int Player_BaseMaxHP;
    private int Player_CurrentMaxHP;
    private int Player_CurrentHP;
    public int Player_BaseMaxMP; //Mana Point
    private int Player_CurrentMaxMP;
    private int Player_CurrentMP;
    public int Player_BaseAP; //Attack Point
    private int Player_CurrentAP;
    public int Player_BaseDP; //Defend Point
    private int Player_CurrentDP;

    private int EquipmentBonusHP = 0;
    private int EquipmentBonusMP = 0;
    private int EquipmentBonusAP = 0;
    private int EquipmentBonusDP = 0;


    private PlayerStatus playerStatus;



    private void Awake()
    {
        Inv_Con = GetComponent<InventoryController>();
        Trans_EquipmentSlotPanel = transform.GetChild(0).GetChild(2);
        Trans_PlayerStatusPanel = Trans_EquipmentSlotPanel.GetChild(0);
        Trans_EquipmentSlotGridGroup = Trans_EquipmentSlotPanel.GetChild(2);
        GetTextCom();
    }

    void Start()
    {
        LoadDatafromAsset();
        SpawnAllTemplateandGetCom();
        DisplayAllEquipmenttoTem();
        GetAllBonusfromEquipments();
        InitializeStatusNum();
        UpdateAllStatusDisplay();
    }

    private void LoadDatafromAsset()
    {
        if (ES_EquipmentSlotAsset == null)
        {
            ES_EquipmentSlotAsset = Resources.Load<EquipmentSlot>("EquipmentSlots_SO/Testing_EquipmentSlot");
        }
        playerStatus = Resources.Load<PlayerStatus>("Player/MainPlayer");
        Player_LV = playerStatus.lv;
        Player_BaseMaxHP = playerStatus.max_hp;
        Player_BaseMaxMP = playerStatus.max_mp;
        Player_BaseAP = playerStatus.attack;
        Player_BaseDP = playerStatus.defense;
    }

    private void SpawnAllTemplateandGetCom()
    {
        for (int i = 0; i < 6; i++)
        {
            SlotPanelTransList.Add(Trans_EquipmentSlotGridGroup.GetChild(i));
            GameObject gb = Instantiate(EquipmentCardTemplate, SlotPanelTransList[i]);
            TemplateComponent.Add(gb.GetComponent<EquipmentCard_DataLoaderAndDisplay>());
        }
    }

    private void DisplayAllEquipmenttoTem()
    {
        ES_EquipmentSlotAsset.UpdateEquipmentList();
        for (int i = 0; i < 6; i++)
        {
            TemplateComponent[i].DisplaytoTemplate(ES_EquipmentSlotAsset.EquipmentSlotList[i]);
        }
    }

    private void GetTextCom()
    {
        TMP_PlayerLV = Trans_PlayerStatusPanel.GetChild(0).GetComponent<TMP_Text>();
        TMP_PlayerHP = Trans_PlayerStatusPanel.GetChild(1).GetComponent<TMP_Text>();
        TMP_PlayerMP = Trans_PlayerStatusPanel.GetChild(2).GetComponent<TMP_Text>();
        TMP_PlayerAP = Trans_PlayerStatusPanel.GetChild(3).GetComponent<TMP_Text>();
        TMP_PlayerDP = Trans_PlayerStatusPanel.GetChild(4).GetComponent<TMP_Text>();
    }

    private void InitializeStatusNum()
    {
        UpdateStatusNum();
        Player_CurrentHP = Player_CurrentMaxHP;
        Player_CurrentMP = Player_CurrentMaxMP;
    }

    private void UpdateStatusNum()
    {
        Player_CurrentMaxHP = Player_BaseMaxHP + EquipmentBonusHP;
        Player_CurrentMaxMP = Player_BaseMaxMP + EquipmentBonusMP;
        Player_CurrentAP = Player_BaseAP + EquipmentBonusAP;
        Player_CurrentDP = Player_BaseDP + EquipmentBonusDP;

        // Watch out! This will be call whenever equipment is changed! Not sure if this will possibly be a problem.
        Player_CurrentHP = Player_CurrentMaxHP;
        Player_CurrentMP = Player_CurrentMaxMP;
    }

    private void UpdateAllStatusDisplay()
    {
        TMP_PlayerLV.text = "LV : " + Player_LV.ToString();
        TMP_PlayerHP.text = "HP : " + Player_BaseMaxHP + " ( " + EquipmentBonusHP + " ) " + " / " + Player_CurrentHP;
        TMP_PlayerMP.text = "MP : " + Player_BaseMaxMP + " ( " + EquipmentBonusMP + " ) " + " / " + Player_CurrentMP;
        TMP_PlayerAP.text = "AP : " + Player_BaseAP + " ( " + EquipmentBonusAP + " ) ";
        TMP_PlayerDP.text = "DP : " + Player_BaseDP + " ( " + EquipmentBonusDP + " ) ";
    }

    private void GetAllBonusfromEquipments()
    {
        EquipmentBonusAP = 0;
        EquipmentBonusMP = 0;
        EquipmentBonusDP = 0;

        foreach (CardData carddata in ES_EquipmentSlotAsset.EquipmentSlotList)
        {
            if (carddata == null) continue;
            switch (carddata._BonusType)
            {
                case BonusType.AttackPoint:
                    EquipmentBonusAP += carddata.BonusNum;
                    break;
                case BonusType.ManaPoint:
                    EquipmentBonusMP += carddata.BonusNum;
                    break;
                case BonusType.DefendPoint:
                    EquipmentBonusDP += carddata.BonusNum;
                    break;
            }

        }
    }

    public void TryTransferCardtoInv(EquipmentCard_DataLoaderAndDisplay card_template)
    {
        if (Inv_Con.ReceiveCard(card_template._CardData))
        {
            switch (card_template.transform.parent.GetSiblingIndex()) // Using the siblingindex to know what is the template equipment type.
            {
                case 0:
                    ES_EquipmentSlotAsset.Weapon = null;
                    break;
                case 1:
                    ES_EquipmentSlotAsset.Armor_Head = null;
                    break;
                case 2:
                    ES_EquipmentSlotAsset.Armor_Body = null;
                    break;
                case 3:
                    ES_EquipmentSlotAsset.Armor_Bottom = null;
                    break;
                case 4:
                    ES_EquipmentSlotAsset.Ornamental_A = null;
                    break;
                case 5:
                    ES_EquipmentSlotAsset.Ornamental_B = null;
                    break;
            }
            ES_EquipmentSlotAsset.UpdateEquipmentList();
            WhenEquipmentChange();
        }
        else
        {
            Debug.Log("Fail to transfer card from equipmentslot to Inv!");
        }
    }


    public bool ReceiveCard(CardData carddata) // Receive a carddata from other list, check if it is ok to receive.
    {
        if (true) // check the player's level is or isn't enough for this equipment. ( future feature )
        {
            switch (carddata._EquipmentType)
            {
                case EquipmentType.Weapon:
                    if (ES_EquipmentSlotAsset.Weapon == null)
                    {
                        ES_EquipmentSlotAsset.Weapon = carddata;
                        ES_EquipmentSlotAsset.UpdateEquipmentList();
                        WhenEquipmentChange();
                        return true;
                    }
                    else
                    {
                        Debug.Log("There is already a weapon! fail to transfer!");
                        return false;
                    }
                case EquipmentType.Armor:
                    switch (carddata._ArmorType)
                    {
                        case ArmorType.Head:
                            if (ES_EquipmentSlotAsset.Armor_Head == null)
                            {
                                ES_EquipmentSlotAsset.Armor_Head = carddata;
                                ES_EquipmentSlotAsset.UpdateEquipmentList();
                                WhenEquipmentChange();
                                return true;
                            }
                            else
                            {
                                Debug.Log("There is already an Armor(Head)! fail to transfer!");
                                return false;
                            }
                        case ArmorType.Body:
                            if (ES_EquipmentSlotAsset.Armor_Body == null)
                            {
                                ES_EquipmentSlotAsset.Armor_Body = carddata;
                                ES_EquipmentSlotAsset.UpdateEquipmentList();
                                WhenEquipmentChange();
                                return true;
                            }
                            else
                            {
                                Debug.Log("There is already an Armor(Body)! fail to transfer!");
                                return false;
                            }
                        case ArmorType.Bottom:
                            if (ES_EquipmentSlotAsset.Armor_Bottom == null)
                            {
                                ES_EquipmentSlotAsset.Armor_Bottom = carddata;
                                ES_EquipmentSlotAsset.UpdateEquipmentList();
                                WhenEquipmentChange();
                                return true;
                            }
                            else
                            {
                                Debug.Log("There is already an Armor(Bottom)! fail to transfer!");
                                return false;
                            }
                    }
                    break;
                case EquipmentType.Ornament:
                    if (ES_EquipmentSlotAsset.Ornamental_A != null && ES_EquipmentSlotAsset.Ornamental_B != null)
                    {
                        Debug.Log("There is already have 2 Ornament! fail to transfer!");
                        return false;
                    }
                    else
                    {
                        if (ES_EquipmentSlotAsset.Ornamental_A == null)
                        {
                            ES_EquipmentSlotAsset.Ornamental_A = carddata;
                            ES_EquipmentSlotAsset.UpdateEquipmentList();
                            WhenEquipmentChange();
                            return true;
                        }
                        else
                        {
                            ES_EquipmentSlotAsset.Ornamental_B = carddata;
                            ES_EquipmentSlotAsset.UpdateEquipmentList();
                            WhenEquipmentChange();
                            return true;
                        }
                    }
            }
            return true;
        }
    }

    private void WhenEquipmentChange()
    {
        ES_EquipmentSlotAsset.UpdateEquipmentList();
        DisplayAllEquipmenttoTem();
        GetAllBonusfromEquipments();
        UpdateStatusNum();
        UpdateAllStatusDisplay();
    }


    public Dictionary<string, int> GetStatus()
    {
        Dictionary<string, int> statusDict = new Dictionary<string, int>();
        statusDict.Add("LV", Player_LV);
        statusDict.Add("CurrentHP", Player_CurrentHP);
        statusDict.Add("MaxHP", Player_CurrentMaxHP);
        statusDict.Add("CurrentMP", Player_CurrentMP);
        statusDict.Add("MaxMP", Player_CurrentMaxMP);
        statusDict.Add("Attack", Player_CurrentAP);
        statusDict.Add("Defense", Player_CurrentDP);

        return statusDict;
    }
}
