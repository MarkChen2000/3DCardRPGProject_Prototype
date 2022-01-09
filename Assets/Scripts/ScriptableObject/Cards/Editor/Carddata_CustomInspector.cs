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
                card.CardCost = EditorGUILayout.IntField("Card Cost", card.CardCost);
                //card._SpellsEffectType = (SpellsEffectType)EditorGUILayout.EnumPopup("Spells Effect Type", card._SpellsEffectType);
                //card.SpellsEffectValue = EditorGUILayout.IntField("Spells Effect Value", card.SpellsEffectValue);
                card.Ability = (CardAbility)EditorGUILayout.ObjectField("Card Ability", card.Ability, typeof(CardAbility), false);
                break;

            case CardType.Equipment:
                card.CardLv = EditorGUILayout.IntField("Card LV", card.CardLv);
                card._EquipmentType = (EquipmentType)EditorGUILayout.EnumPopup("Equipment Type", card._EquipmentType);
                card.EquipmentPrefab = (GameObject)EditorGUILayout.ObjectField("Equipment Prefab", card.EquipmentPrefab, typeof(GameObject), false);

                card.EquipmentBonusHP = EditorGUILayout.FloatField(new GUIContent("Equipment HP Bonus", "Recommand set betwwen 0~3"), card.EquipmentBonusHP);
                card.EquipmentBonusPW = EditorGUILayout.FloatField(new GUIContent("Equipment PW Bonus", "Recommand set betwwen 0~3"), card.EquipmentBonusPW);
                card.EquipmentBonusMP = EditorGUILayout.FloatField(new GUIContent("Equipment MP Bonus", "Recommand set betwwen 0~3"), card.EquipmentBonusMP);
                card.EquipmentBonusRT = EditorGUILayout.FloatField(new GUIContent("Equipment RT Bonus", "Recommand set betwwen 0~5"), card.EquipmentBonusRT);
                card.EquipmentBonusSP = EditorGUILayout.FloatField(new GUIContent("Equipment SP Bonus", "Recommand set betwwen 0~50"), card.EquipmentBonusSP);

                switch ( card._EquipmentType )
                {
                    case EquipmentType.Weapon:
                        card.WeaponAP = EditorGUILayout.IntField(new GUIContent("WeaponAP", "Have to set above 1, recommand set betwwen 1~100, It's mean the preotect percentage from damage"), card.WeaponAP);
                        card.WeaponCR = EditorGUILayout.IntField("WeaponCR", card.WeaponCR);
                        break;
                    case EquipmentType.Armor:
                        card._ArmorType = (ArmorType)EditorGUILayout.EnumPopup("Armor Type", card._ArmorType);
                        card.ArmorDP = EditorGUILayout.IntField("ArmorDP", card.ArmorDP);
                        card.ArmorDSP = EditorGUILayout.IntField("ArmorDSP", card.ArmorDSP);
                        break;
                }
                break;
        }
        EditorUtility.SetDirty(card);
    }


}
