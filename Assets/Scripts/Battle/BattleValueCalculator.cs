using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType // Every attack will either magic-type or physics-type. But enemy's attack have no difference.
{
    Null, Magic, Physics
}

public class BattleValueCalculator : MonoBehaviour
{

    private PlayerStatusController PlayerStatusCon;
    private EquipmentSlotController EquipmentSlotCon;

    [Header("This magnification will diverse magic and physics attack.")]
    public float MagicAttackMagnification = 1.5f;

    private void Start()
    {
        PlayerStatusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
        EquipmentSlotCon = GameObject.Find("InventoryManager").GetComponent<EquipmentSlotController>();
    }
    // This function will do the player's and enemy's damage value calculation uniformly, All the damage formula can adjust at here.

    public int PlayerDamageCalculate(AttackType type, int spellsvalue) 
    {
    // Attacktype mean what is this attack type.
    // if the attack type is physics, the spellsvalue doesn't matter.
        float finaldamage = 0;

        switch ( type )
        {
            case AttackType.Magic:
                finaldamage = (PlayerStatusCon.baseMP + EquipmentSlotCon.EquipmentBonusMP) * spellsvalue * MagicAttackMagnification;
                break;
            case AttackType.Physics:
                if (EquipmentSlotCon.Weapon == null) finaldamage = PlayerStatusCon.basePW;
                else finaldamage = (PlayerStatusCon.basePW + EquipmentSlotCon.EquipmentBonusPW) * EquipmentSlotCon.Weapon.WeaponAP;
                break;
            case AttackType.Null:
                Debug.Log("This player's attack didnt have type!");
                break;
        }

        return (int)Mathf.Round(finaldamage);
    }

    public int PlayerTakeDamageCalculate(int damagevalue) // call whenever player get attack;
    {
        float finaldamage = 0;

        finaldamage = damagevalue - EquipmentSlotCon.Armor_DP;

        return (int)Mathf.Round(finaldamage);
    }

    public int EnemyDamageCalculate(int damagevalue) // call whenever enemy attack. 
    {
        float finaldamage = 0;

        finaldamage = damagevalue;

        return (int)Mathf.Round(finaldamage);
    }

    public int EnemyTakeDamageCalculate(int damagevalue) // call whenever enemy get attack.
    {
        float finaldamage = 0;

        finaldamage = damagevalue;

        return (int)Mathf.Round(finaldamage);
    }

}
