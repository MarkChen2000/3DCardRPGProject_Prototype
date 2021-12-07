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
    public PlayerStatus playerStatus;

    private Transform Trans_EquipmentSlotPanel;
    private Transform Trans_PlayerStatusPanel;
    private Transform Trans_EquipmentSlotGridGroup;
    private TMP_Text TMP_PlayerLV, TMP_PlayerHP, TMP_PlayerPW, TMP_PlayerMP, TMP_PlayerMananRT, TMP_WeaponAPnCR, TMP_ArmorDP;

    [Space]

    public int Player_LV; // Level
    public int Player_BaseMaxHP = 100; //Health Point
    private int Player_BaseMP; 
    public int Player_BasePW; // Power
    public int Player_BaseMaxMana;
    public int Player_BaseManaRT; // Mana Recovery Time

    private int Weapon_AP = 0;
    private int Weapon_CR = 0;
    private int Armor_DP = 0;
    private int EquipmentBonusHP = 0;
    private int EquipmentBonusPW = 0;
    private int EquipmentBonusMP = 0;

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
        GetEquipmentValue();
        InitializeStatusNum();
        UpdateAllStatusDisplay();
    }

    private void LoadDatafromAsset()
    {
        if (ES_EquipmentSlotAsset == null)
        {
            ES_EquipmentSlotAsset = Resources.Load<EquipmentSlot>("EquipmentSlots_SO/Testing_EquipmentSlot");
        }
        if ( playerStatus == null )
        {
            playerStatus = Resources.Load<PlayerStatus>("Player/MainPlayer");
        }
        LoadPlayerStatusValue();
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
        TMP_PlayerPW = Trans_PlayerStatusPanel.GetChild(2).GetComponent<TMP_Text>();
        TMP_PlayerMP = Trans_PlayerStatusPanel.GetChild(3).GetComponent<TMP_Text>();
        TMP_PlayerMananRT = Trans_PlayerStatusPanel.GetChild(4).GetComponent<TMP_Text>();
        TMP_WeaponAPnCR = Trans_PlayerStatusPanel.GetChild(5).GetComponent<TMP_Text>();
        TMP_ArmorDP = Trans_PlayerStatusPanel.GetChild(6).GetComponent<TMP_Text>();
    }

    private void InitializeStatusNum()
    {
        UpdateStatusValue();
    }

    private void LoadPlayerStatusValue()
    {
        Player_LV = playerStatus.LV;
        Player_BaseMaxHP = playerStatus.baseMaxHP;
        Player_BaseMP = playerStatus.baseMP;
        Player_BasePW = playerStatus.basePW;
        Player_BaseMaxMana = playerStatus.baseMaxMana;
        Player_BaseManaRT = playerStatus.baseManaRT;
    }

    private void UpdateStatusValue()
    {
        LoadPlayerStatusValue();
        Player_BaseMaxHP = Player_BaseMaxHP + EquipmentBonusHP;
        Player_BasePW = Player_BasePW + EquipmentBonusPW;
        Player_BaseMP = Player_BaseMP + EquipmentBonusMP;
    }

    private void UpdateAllStatusDisplay()
    {
        TMP_PlayerLV.text = "LV : " + Player_LV;
        TMP_PlayerHP.text = "HP : " + Player_BaseMaxHP + " ( " + EquipmentBonusHP + " ) " ;
        TMP_PlayerPW.text = "Power : " + Player_BasePW;
        TMP_PlayerMP.text = "MP : " + Player_BaseMP + " ( " + EquipmentBonusMP + " ) " ;
        TMP_PlayerMananRT.text = "Mana/Recovery : " + Player_BaseMaxMana + " ( " + EquipmentBonusMP + " ) /" + Player_BaseManaRT;
        TMP_WeaponAPnCR.text = "Waepon AP/CR : " + Weapon_AP + "/" + Weapon_CR + "%";
        TMP_ArmorDP.text = "Armor DP : " + Armor_DP ;
    }

    private void GetEquipmentValue()
    {
        Weapon_AP = 0;
        Weapon_CR = 0;
        Armor_DP = 0;
        EquipmentBonusPW = 0;
        EquipmentBonusMP = 0;
        EquipmentBonusHP = 0;

        foreach (CardData carddata in ES_EquipmentSlotAsset.EquipmentSlotList)
        {
            if (carddata == null) continue;
            switch (carddata._EquipmentType)
            {
                case EquipmentType.Weapon:
                    Weapon_AP = carddata.WaeponAP;
                    Weapon_CR = carddata.WeaponCR;
                    continue;
                case EquipmentType.Armor:
                    Armor_DP += carddata.ArmorDP;
                    break;
                case EquipmentType.Ornament:

                    break;
                case EquipmentType.NotEquip:
                    Debug.Log("This is not Equip!");
                    break;
            }

            switch ( carddata._BonusType )
            {
                case BonusType.HP:
                    EquipmentBonusHP += carddata.BonusNum;
                    break;
                case BonusType.PW:
                    EquipmentBonusPW += carddata.BonusNum;
                    break;
                case BonusType.MP:
                    EquipmentBonusMP += carddata.BonusNum;
                    break;
                case BonusType.Null:

                    break;
                case BonusType.NotEquip:
                    Debug.Log("This is not Equipment!");
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
                    ES_EquipmentSlotAsset.Ornament_A = null;
                    break;
                case 5:
                    ES_EquipmentSlotAsset.Ornament_B = null;
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
                                WhenEquipmentChange();
                                return true;
                            }
                            else
                            {
                                Debug.Log("There is already an Armor(Bottom)! fail to transfer!");
                                return false;
                            }
                        case ArmorType.NotArmor:
                            Debug.Log("This is not Armor!");
                            break;
                    }
                    break;
                case EquipmentType.Ornament:
                    if (ES_EquipmentSlotAsset.Ornament_A != null && ES_EquipmentSlotAsset.Ornament_B != null)
                    {
                        Debug.Log("There is already have 2 Ornament! fail to transfer!");
                        return false;
                    }
                    else
                    {
                        if (ES_EquipmentSlotAsset.Ornament_A == null)
                        {
                            ES_EquipmentSlotAsset.Ornament_A = carddata;
                            WhenEquipmentChange();
                            return true;
                        }
                        else
                        {
                            ES_EquipmentSlotAsset.Ornament_B = carddata;
                            WhenEquipmentChange();
                            return true;
                        }
                    }
                case EquipmentType.NotEquip:
                    Debug.Log("This is not Equipment!");
                    break;
            }
            return true;
        }
    }

    private void WhenEquipmentChange()
    {
        ES_EquipmentSlotAsset.UpdateEquipmentList();
        DisplayAllEquipmenttoTem();
        GetEquipmentValue();
        UpdateStatusValue();
        UpdateAllStatusDisplay();
    }
}
