using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Spells, Equipment
}

public enum BonusType
{
    AttackPoint, DefendPoint, ManaPoint,
}

public enum EquipmentType
{
    Weapon, Armor, Ornament
}

public enum ArmorType
{
    Head, Body, Bottom
}

[CreateAssetMenu(fileName = "New Card Data", menuName = "Inventory/CardData")]
public class CardData : ScriptableObject, IComparable // IComparable is for using self-made sort function.
{
    [Header("Base Card Data")]
    public int CardID;
    public Sprite Card_Image;
    public string CardName = "DefaultName";
    public string CardDescription = "DefaultDes";
    public CardType _CardType;

    [Space]
    [Header("For Spell Card")]
    public int CardCost = 0;

    [Space]
    [Header("For Equipment Card")]
    public int CardLv = 0;
    public EquipmentType _EquipmentType;
    public ArmorType _ArmorType;
    public BonusType _BonusType;
    public int BonusNum = 0;

    // public int CardHoldNum = 1; // doesnt need holdnum now because that one element in list is meaning one single card.


    public void ExecuteCardFunction(string cardName, GameObject target, PlayerStatus playerStatus) // run the card function at here.
    {
        if (CardName == "Fire Ball")
        {
            if (target.tag == "Monster")
            {
                target.GetComponent<Monster_StatusController>().beAttacked(6 + playerStatus.attack);
            }
        }
    }

    public int CompareTo(object obj) // This is how to sort data of this type (Carddata).
    {
        CardData carddata = (CardData)obj; // Change the type of obj to CardData

        if (_CardType == carddata._CardType)
        {
            switch (_CardType)
            {
                case CardType.Spells:
                    if (CardCost == carddata.CardCost) // If the Cost are same, compare with Name.
                    {
                        return CardName.CompareTo(carddata.CardName);
                    }
                    return CardCost - carddata.CardCost;
                case CardType.Equipment:
                    if (CardLv == carddata.CardLv) // If the Lv are same, compare with Name.
                    {
                        return CardName.CompareTo(carddata.CardName);
                    }
                    return CardLv - carddata.CardLv;
            }
        }

        if (_CardType == CardType.Spells && carddata._CardType == CardType.Equipment) return 1;
        else return -1;
    }
}
