using System;
using UnityEngine;

public enum CardType // Every card will be either spells or equipment.
{
    Spells, Equipment
}

public enum SpellsEffectType
{
    Damage, Heal, Duration, Distance
}

public enum BonusType // Only Equipment have Bonus Value
{
    NotEquip, Null, PW, HP, MP, SP, RT
}

public enum EquipmentType
{
    Weapon, Armor, Ornament
}

public enum ArmorType
{
    Head, Body, Bottom,
}

[CreateAssetMenu(fileName = "New Card Data", menuName = "Inventory/CardData")]
public class CardData : ScriptableObject, IComparable // IComparable is for using self-made sort function.
{
    [Header("Base Card Data")]
    public int CardID;
    public Sprite Card_ImageSprite;
    public string CardName = "DefaultName";
    [TextArea]
    public string CardDescription = "DefaultDes";
    public CardType _CardType = CardType.Spells;

    [Space]
    [Header("For Spell Card")]
    public int CardCost = 0;
    public SpellsEffectType _SpellsEffectType = SpellsEffectType.Damage;
    public int SpellsEffectValue = 0;

    [Space]
    [Header("For Equipment Card")]
    public int CardLv = 0;
    public EquipmentType _EquipmentType;
    [Tooltip("Recommand set betwwen 0~3")] public float EquipmentBonusHP = 0;
    [Tooltip("Recommand set betwwen 0~3")] public float EquipmentBonusPW = 0;
    [Tooltip("Recommand set betwwen 0~3")] public float EquipmentBonusMP = 0;
    [Tooltip("Recommand set betwwen 0~5")] public float EquipmentBonusRT = 0;
    [Tooltip("Recommand set betwwen 0~50")] public float EquipmentBonusSP = 0;

    [Header("For Waepon")]
    public int WeaponAP = 0;
    public int WeaponCR = 0; //Critical Rate

    [Header("For Armor")]
    public ArmorType _ArmorType;
    [Tooltip("Have to set above 1, recommand set betwwen 1~100, It's mean the preotect percentage from damage")] 
    public int ArmorDP = 1;
    public int ArmorDSP = 0; // the debuff for the speed.


    // public int CardHoldNum = 1; // doesnt need holdnum now because that one element in list is meaning one single card.

    public void ExecuteCardFunction(string cardName, Vector3 direction, PlayerStatusController playerStatusCon, GameObject prefab) // run the card function at here.
    {
        // 12.12 change playerstatus to playerstatuscontroller for the SO usage change.
        if (CardName == "Fire Ball")
        {
            Debug.Log("Fireball casted toward: " + direction);
            prefab = GameObject.Find("BattleManager").GetComponent<PrefabController>().fireballPrefab;
            GameObject newPrefab = Instantiate(prefab);
            newPrefab.GetComponent<FireballController>().Shoot(direction, playerStatusCon);
        }
        else if (CardName == "Heal Potion" || CardName == "Quick Heal")
        {
            playerStatusCon.UpdateStatus("currentHP", 6);
            prefab = GameObject.Find("BattleManager").GetComponent<PrefabController>().healPotionPrefab;
            GameObject newPrefab = Instantiate(prefab);
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
