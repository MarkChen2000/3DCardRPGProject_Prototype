using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardData))]
public class Carddata_CustomInspector : Editor
{
    public override void OnInspectorGUI() // when override this function, will not show the variable that public in original script.
    {
        //base.OnInspectorGUI();

        CardData card = (CardData)target; // target will always mean that the target of what is currently be custom it's inspector.

        card.CardID = EditorGUILayout.IntField("Card ID", card.CardID);
        card.Card_ImageSprite = (Sprite)EditorGUILayout.ObjectField("Card Image Sprite",card.Card_ImageSprite, typeof(Sprite), false);
        card.CardName = EditorGUILayout.TextField("Card Name", card.CardName);
        card.CardDescription = EditorGUILayout.TextField("Card Description", card.CardDescription);

        card._CardType = (CardType)EditorGUILayout.EnumPopup("Card type", card._CardType);
        switch ( card._CardType )
        {
            case CardType.Spells:
                EditorGUI.BeginDisabledGroup(false);
                card.CardCost = EditorGUILayout.IntField("Card Cost", card.CardCost);
                card._SpellsEffectType = (SpellsEffectType)EditorGUILayout.EnumPopup("Spells Effect Type", card._SpellsEffectType);
                card.SpellsEffectValue = EditorGUILayout.IntField("Spells Effect Value", card.SpellsEffectValue);
                EditorGUI.EndDisabledGroup();
                break;

            case CardType.Equipment:
                EditorGUI.BeginDisabledGroup(false);
                card.CardLv = EditorGUILayout.IntField("Card LV", card.CardLv);
                card.EquipmentBonusHP = EditorGUILayout.IntField("Equipment HP Bonus", card.EquipmentBonusHP);
                card.EquipmentBonusPW = EditorGUILayout.IntField("Equipment PW Bonus", card.EquipmentBonusPW);
                card.EquipmentBonusMP = EditorGUILayout.IntField("Equipment MP Bonus", card.EquipmentBonusMP);
                card.EquipmentBonusSP = EditorGUILayout.FloatField("Equipment SP Bonus", card.EquipmentBonusSP);
                card.EquipmentBonusRT = EditorGUILayout.FloatField("Equipment RT Bonus", card.EquipmentBonusRT);

                card._EquipmentType = (EquipmentType)EditorGUILayout.EnumPopup("Equipment Type", card._EquipmentType);
                switch ( card._EquipmentType )
                {
                    case EquipmentType.NotEquip:
                        break;
                    case EquipmentType.Weapon:
                        EditorGUI.BeginDisabledGroup(false);
                        card.WeaponAP = EditorGUILayout.IntField("WeaponAP", card.WeaponAP);
                        card.WeaponCR = EditorGUILayout.IntField("WeaponCR", card.WeaponCR);
                        EditorGUI.EndDisabledGroup();
                        break;
                    case EquipmentType.Armor:
                        EditorGUI.BeginDisabledGroup(false);
                        card._ArmorType = (ArmorType)EditorGUILayout.EnumPopup("Armor Type", card._ArmorType);
                        card.ArmorDP = EditorGUILayout.IntField("ArmorDP", card.ArmorDP);
                        card.ArmorDSP = EditorGUILayout.FloatField("ArmorDSP", card.ArmorDSP);
                        EditorGUI.EndDisabledGroup();
                        break;
                }
                EditorGUI.EndDisabledGroup();
                break;
        }

    }


}
