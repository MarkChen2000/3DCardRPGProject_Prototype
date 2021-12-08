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

    /*public int Player_LV; // Level
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
    private int EquipmentBonusMP = 0;*/

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
        ES_EquipmentSlotAsset.UpdateEquipmentValue();
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
        /*ES_EquipmentSlotAsset.UpdateEquipmentList();
        for (int i = 0; i < 6; i++)
        {
            TemplateComponent[i].DisplaytoTemplate(ES_EquipmentSlotAsset.EquipmentSlotList[i]);
        }*/
        TemplateComponent[0].DisplaytoTemplate(ES_EquipmentSlotAsset.Weapon);
        TemplateComponent[1].DisplaytoTemplate(ES_EquipmentSlotAsset.Armor_Head);
        TemplateComponent[2].DisplaytoTemplate(ES_EquipmentSlotAsset.Armor_Body);
        TemplateComponent[3].DisplaytoTemplate(ES_EquipmentSlotAsset.Armor_Bottom);
        TemplateComponent[4].DisplaytoTemplate(ES_EquipmentSlotAsset.Ornament_A);
        TemplateComponent[5].DisplaytoTemplate(ES_EquipmentSlotAsset.Ornament_B);
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
        AddEquipBonusValue();
    }

    private void AddEquipBonusValue()
    {
        playerStatus.currentMaxHP = playerStatus.baseMaxHP + ES_EquipmentSlotAsset.EquipmentBonusHP;
        playerStatus.currentPW = playerStatus.basePW + ES_EquipmentSlotAsset.EquipmentBonusPW;
        playerStatus.currentMP = playerStatus.baseMP + ES_EquipmentSlotAsset.EquipmentBonusMP;
    }

    private void UpdateAllStatusDisplay()
    {
        TMP_PlayerLV.text = "LV: " + playerStatus.LV + " ( " + playerStatus.nextLVEXP + " / " + playerStatus.EXP + " ) ";
        TMP_PlayerHP.text = "HP: " + playerStatus.currentMaxHP + " ( " + ES_EquipmentSlotAsset.EquipmentBonusHP + " ) " ;
        TMP_PlayerPW.text = "Power: " + playerStatus.currentPW + " ( " + ES_EquipmentSlotAsset.EquipmentBonusPW + " ) ";
        TMP_PlayerMP.text = "MP: " + playerStatus.currentMP + " ( " + ES_EquipmentSlotAsset.EquipmentBonusMP + " ) " ;
        TMP_PlayerMananRT.text = "Mana/Recovery: " + playerStatus.baseMaxMana + " / " + playerStatus.baseManaRT + "s";
        TMP_WeaponAPnCR.text = "WeaponAP/CR: " + ES_EquipmentSlotAsset.Weapon_AP + " / " + ES_EquipmentSlotAsset.Weapon_CR + "%";
        TMP_ArmorDP.text = "ArmorDP: " + ES_EquipmentSlotAsset.Armor_DP ;
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
        DisplayAllEquipmenttoTem();
        ES_EquipmentSlotAsset.UpdateEquipmentValue();
        AddEquipBonusValue();
        UpdateAllStatusDisplay();
    }
}
