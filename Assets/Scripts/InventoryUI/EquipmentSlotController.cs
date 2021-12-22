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
    private PlayerStatusController PlayerStatusCon;

    private Transform Trans_InventoryPanelBG;
    private Transform Trans_PlayerStatusPanel;
    private Transform Trans_EquipmentSlotGridGroup;
    private TMP_Text TMP_PlayerLV, TMP_PlayerHP, TMP_PlayerPW, TMP_PlayerMP, TMP_PlayerMananRT, TMP_WeaponAPnCR, TMP_ArmorDPnDSP;

    public EquipmentSlot _EquipmentSlotAsset;
    public CardData Weapon, Armor_Head, Armor_Body, Armor_Bottom, Ornament_A, Ornament_B;

    [HideInInspector] public int Weapon_AP = 0;
    [HideInInspector] public int Weapon_CR = 0;
    [HideInInspector] public int Armor_DP = 0;
    [HideInInspector] public float Armor_DSP = 0;
    [HideInInspector] public int EquipmentBonusHP = 0;
    [HideInInspector] public int EquipmentBonusPW = 0;
    [HideInInspector] public int EquipmentBonusMP = 0;
    [HideInInspector] public float EquipmentBonusSP = 0;
    [HideInInspector] public float EquipmentBonusRT = 0;


    private void Awake()
    {
        PlayerStatusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
        Inv_Con = GetComponent<InventoryController>();
        Trans_InventoryPanelBG = transform.GetChild(0).GetChild(0);
        Trans_PlayerStatusPanel = Trans_InventoryPanelBG.GetChild(2).GetChild(0);
        Trans_EquipmentSlotGridGroup = Trans_InventoryPanelBG.GetChild(2).GetChild(2);
        GetTextCom();

        if (_EquipmentSlotAsset == null) _EquipmentSlotAsset = Resources.Load<EquipmentSlot>("EquipmentSlots_SO/Testing_EquipmentSlot");
        // Load initial EquipmentSlot asset first, this may be replaced by Save and Load System before build!
    }

    void Start()
    {
        InitializeLoadinData();
        SpawnAllTemplateandGetCom();
        DisplayAllEquipmenttoTem();
        UpdateEquipmentValue();
        InitializeStatusNum();
        UpdateAllStatusDisplay();
    }

    public void SaveandLoadEquipSlot(bool SorL)
    {
        // save and load system.
        if ( SorL )
        { }
        else
        { }
    }

    private void InitializeLoadinData()
    {
        Weapon = _EquipmentSlotAsset.Weapon;
        Armor_Head = _EquipmentSlotAsset.Armor_Head;
        Armor_Body = _EquipmentSlotAsset.Armor_Body;
        Armor_Bottom = _EquipmentSlotAsset.Armor_Bottom;
        Ornament_A = _EquipmentSlotAsset.Ornament_A;
        Ornament_B = _EquipmentSlotAsset.Ornament_B;
    }

    private void UpdateEquipmentValue()
    {
        List<CardData> EquipmentSlotList = new List<CardData>();
        EquipmentSlotList.Clear();
        EquipmentSlotList.Add(Weapon);
        EquipmentSlotList.Add(Armor_Head);
        EquipmentSlotList.Add(Armor_Body);
        EquipmentSlotList.Add(Armor_Bottom);
        EquipmentSlotList.Add(Ornament_A);
        EquipmentSlotList.Add(Ornament_B);

        Weapon_AP = 0;
        Weapon_CR = 0;
        Armor_DP = 0;
        Armor_DSP = 0;
        EquipmentBonusHP = 0;
        EquipmentBonusPW = 0;
        EquipmentBonusMP = 0;
        EquipmentBonusSP = 0;
        EquipmentBonusRT = 0;

        foreach (CardData carddata in EquipmentSlotList)
        {
            if (carddata == null) continue;
            switch (carddata._EquipmentType)
            {
                case EquipmentType.Weapon:
                    Weapon_AP = carddata.WeaponAP;
                    Weapon_CR = carddata.WeaponCR;
                    break;
                case EquipmentType.Armor:
                    Armor_DP += carddata.ArmorDP;
                    Armor_DSP += carddata.ArmorDSP;
                    break;
                case EquipmentType.Ornament:

                    break;
                case EquipmentType.NotEquip:
                    Debug.Log("This is not Equipment!");
                    break;
            }

            EquipmentBonusHP += carddata.EquipmentBonusHP;
            EquipmentBonusPW += carddata.EquipmentBonusPW;
            EquipmentBonusMP += carddata.EquipmentBonusMP;
            EquipmentBonusSP += carddata.EquipmentBonusSP;
            EquipmentBonusRT += carddata.EquipmentBonusRT;
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
        TemplateComponent[0].DisplaytoTemplate(Weapon);
        TemplateComponent[1].DisplaytoTemplate(Armor_Head);
        TemplateComponent[2].DisplaytoTemplate(Armor_Body);
        TemplateComponent[3].DisplaytoTemplate(Armor_Bottom);
        TemplateComponent[4].DisplaytoTemplate(Ornament_A);
        TemplateComponent[5].DisplaytoTemplate(Ornament_B);
    }

    private void GetTextCom()
    {
        TMP_PlayerLV = Trans_PlayerStatusPanel.GetChild(0).GetComponent<TMP_Text>();
        TMP_PlayerHP = Trans_PlayerStatusPanel.GetChild(1).GetComponent<TMP_Text>();
        TMP_PlayerPW = Trans_PlayerStatusPanel.GetChild(2).GetComponent<TMP_Text>();
        TMP_PlayerMP = Trans_PlayerStatusPanel.GetChild(3).GetComponent<TMP_Text>();
        TMP_PlayerMananRT = Trans_PlayerStatusPanel.GetChild(4).GetComponent<TMP_Text>();
        TMP_WeaponAPnCR = Trans_PlayerStatusPanel.GetChild(5).GetComponent<TMP_Text>();
        TMP_ArmorDPnDSP = Trans_PlayerStatusPanel.GetChild(6).GetComponent<TMP_Text>();
    }

    private void InitializeStatusNum()
    {
        AddEquipBonusValuetoPlayerStatus();
    }

    public void AddEquipBonusValuetoPlayerStatus()
    {
        PlayerStatusCon.currentMaxHP = PlayerStatusCon.baseMaxHP + EquipmentBonusHP;
        PlayerStatusCon.currentPW = PlayerStatusCon.basePW + EquipmentBonusPW;
        PlayerStatusCon.currentMP = PlayerStatusCon.baseMP + EquipmentBonusMP;
        PlayerStatusCon.currentMaxSpeed = PlayerStatusCon.baseMaxSpeed + EquipmentBonusSP - Armor_DSP;
        PlayerStatusCon.currentMaxManaRT = PlayerStatusCon.baseManaRT + EquipmentBonusRT;
    }

    private void UpdateAllStatusDisplay()
    {
        TMP_PlayerLV.text = "LV: " + PlayerStatusCon.LV + " ( " + PlayerStatusCon.nextLVEXP + " / " + PlayerStatusCon.EXP + " ) ";
        TMP_PlayerHP.text = "HP: " + PlayerStatusCon.currentMaxHP + " ( " + EquipmentBonusHP + " ) " ;
        TMP_PlayerPW.text = "Power: " + PlayerStatusCon.currentPW + " ( " + EquipmentBonusPW + " ) ";
        TMP_PlayerMP.text = "MP: " + PlayerStatusCon.currentMP + " ( " + EquipmentBonusMP + " ) " ;
        TMP_PlayerMananRT.text = "Mana/Recovery: " + PlayerStatusCon.baseMaxMana + " / " + PlayerStatusCon.currentMaxManaRT  + " ( " + EquipmentBonusRT + " ) s";
        TMP_WeaponAPnCR.text = "WeaponAP/CR: " + Weapon_AP + " / " + Weapon_CR + "%";
        TMP_ArmorDPnDSP.text = "ArmorDP/DPS: " + Armor_DP + " / " + Armor_DSP;
    }

    public void TryTransferCardtoInv(EquipmentCard_DataLoaderAndDisplay card_template)
    {
        if (Inv_Con.ReceiveCard(card_template._CardData))
        {
            switch (card_template.transform.parent.GetSiblingIndex()) // Using the siblingindex to know what is the template equipment type.
            {
                case 0:
                    Weapon = null;
                    break;
                case 1:
                    Armor_Head = null;
                    break;
                case 2:
                    Armor_Body = null;
                    break;
                case 3:
                    Armor_Bottom = null;
                    break;
                case 4:
                    Ornament_A = null;
                    break;
                case 5:
                    Ornament_B = null;
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
                    if (Weapon == null)
                    {
                        Weapon = carddata;
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
                            if (Armor_Head == null)
                            {
                                Armor_Head = carddata;
                                WhenEquipmentChange();
                                return true;
                            }
                            else
                            {
                                Debug.Log("There is already an Armor(Head)! fail to transfer!");
                                return false;
                            }
                        case ArmorType.Body:
                            if (Armor_Body == null)
                            {
                                Armor_Body = carddata;
                                WhenEquipmentChange();
                                return true;
                            }
                            else
                            {
                                Debug.Log("There is already an Armor(Body)! fail to transfer!");
                                return false;
                            }
                        case ArmorType.Bottom:
                            if (Armor_Bottom == null)
                            {
                                Armor_Bottom = carddata;
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
                    if (Ornament_A != null && Ornament_B != null)
                    {
                        Debug.Log("There is already have 2 Ornament! fail to transfer!");
                        return false;
                    }
                    else
                    {
                        if (Ornament_A == null)
                        {
                            Ornament_A = carddata;
                            WhenEquipmentChange();
                            return true;
                        }
                        else
                        {
                            Ornament_B = carddata;
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
        UpdateEquipmentValue();
        AddEquipBonusValuetoPlayerStatus();
        UpdateAllStatusDisplay();
    }
}
