using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EquipmentSlot",menuName = "Inventory/EquipmentSlot")]
public class EquipmentSlot : ScriptableObject
{
    public CardData Weapon, Armor_Head, Armor_Body, Armor_Bottom, Ornament_A, Ornament_B;
    private List<CardData> EquipmentSlotList = new List<CardData>(); // using list for visialize and because of the sequentiality.
    // private Dictionary<string,CardData> _EquipmentSlotDic = new Dictionary<string, CardData>();
    // Dictionary didnt have sequentiality and visialbility, and overlapping the feature of the normal variable.
    // e.g. they are all like a string and store in a Carddata variable.
    // so didn't have to use dictionary.

    [HideInInspector] public int Weapon_AP = 0;
    [HideInInspector] public int Weapon_CR = 0;
    [HideInInspector] public int Armor_DP = 0;
    [HideInInspector] public int EquipmentBonusHP = 0;
    [HideInInspector] public int EquipmentBonusPW = 0;
    [HideInInspector] public int EquipmentBonusMP = 0;

    private void OnEnable()
    {
        InitializeEquipmentList();
    }

    private void InitializeEquipmentList()
    {
        /*_EquipmentSlotDic.Clear();
        _EquipmentSlotDic.Add("Weapon", Weapon);
        _EquipmentSlotDic.Add("Armor_Head", Armor_Head);
        _EquipmentSlotDic.Add("Armor_Body", Armor_Body);
        _EquipmentSlotDic.Add("Armor_Bottom", Armor_Bottom);
        _EquipmentSlotDic.Add("Ornament_A", Ornament_A);
        _EquipmentSlotDic.Add("Ornament_B", Ornament_B);*/

        EquipmentSlotList.Clear();
        EquipmentSlotList.Add(Weapon);
        EquipmentSlotList.Add(Armor_Head);
        EquipmentSlotList.Add(Armor_Body);
        EquipmentSlotList.Add(Armor_Bottom);
        EquipmentSlotList.Add(Ornament_A);
        EquipmentSlotList.Add(Ornament_B);
    }

    private void UpdateEquipmentList()
    {
        /*_EquipmentSlotDic["Weapon"] = Weapon;
        _EquipmentSlotDic["Armor_Head"] = Armor_Head;
        _EquipmentSlotDic["Armor_Body"] = Armor_Body;
        _EquipmentSlotDic["Armor_Bottom"] = Armor_Bottom;
        _EquipmentSlotDic["Ornament_A"] = Ornament_A;
        _EquipmentSlotDic["Ornament_B"] = Ornament_B;*/

        EquipmentSlotList[0] = Weapon;
        EquipmentSlotList[1] = Armor_Head;
        EquipmentSlotList[2] = Armor_Body;
        EquipmentSlotList[3] = Armor_Bottom;
        EquipmentSlotList[4] = Ornament_A;
        EquipmentSlotList[5] = Ornament_B;
    }

    public void UpdateEquipmentValue()
    {
        UpdateEquipmentList();

        Weapon_AP = 0;
        Weapon_CR = 0;
        Armor_DP = 0;
        EquipmentBonusPW = 0;
        EquipmentBonusMP = 0;
        EquipmentBonusHP = 0;

        foreach (CardData carddata in EquipmentSlotList)
        {
            if (carddata == null) continue;
            switch (carddata._EquipmentType)
            {
                case EquipmentType.Weapon:
                    Weapon_AP = carddata.WeaponAP;
                    Weapon_CR = carddata.WeaponCR;
                    continue;
                case EquipmentType.Armor:
                    Armor_DP += carddata.ArmorDP;
                    break;
                case EquipmentType.Ornament:

                    break;
                case EquipmentType.NotEquip:
                    Debug.Log("This is not Equipment!");
                    break;
            }

            switch (carddata._BonusType)
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
}
