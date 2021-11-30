using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EquipmentSlot",menuName = "Inventory/EquipmentSlot")]
public class EquipmentSlot : ScriptableObject
{
    public CardData Weapon, Armor_Head, Armor_Body, Armor_Bottom, Ornamental_A, Ornamental_B;
    public List<CardData> EquipmentSlotList = new List<CardData>();

    public void InitializeEquipmentList()
    {
        EquipmentSlotList.Clear();
        EquipmentSlotList.Add(Weapon);
        EquipmentSlotList.Add(Armor_Head);
        EquipmentSlotList.Add(Armor_Body);
        EquipmentSlotList.Add(Armor_Bottom);
        EquipmentSlotList.Add(Ornamental_A);
        EquipmentSlotList.Add(Ornamental_B);
    }

    public void UpdateEquipmentList()
    {
        EquipmentSlotList[0] = Weapon;
        EquipmentSlotList[1] = Armor_Head;
        EquipmentSlotList[2] = Armor_Body;
        EquipmentSlotList[3] = Armor_Bottom;
        EquipmentSlotList[4] = Ornamental_A;
        EquipmentSlotList[5] = Ornamental_B;
    }

}
