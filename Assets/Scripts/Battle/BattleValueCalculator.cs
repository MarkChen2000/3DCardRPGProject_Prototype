using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType // Every attack will either magic-type or physics-type. But enemy's attack have no difference.
{
    Null, Magic, Physics
}

public class BattleValueCalculator : MonoBehaviour
{

    public PlayerStatus _PlayerStatus;
    public EquipmentSlot _EquipmentSlot;

    [Header("This magnification will diverse magic and physics attack.")]
    public float MagicAttackMagnification = 1.5f;

    private void Start()
    {
        if ( _PlayerStatus==null )
        {
            _PlayerStatus = Resources.Load<PlayerStatus>("Player/MainPlayer");
        }
        if ( _EquipmentSlot==null )
        {
            _EquipmentSlot = Resources.Load<EquipmentSlot>("EquipmentSlots_SO/Testing_EquipmentSlot");
        }
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
                finaldamage = (_PlayerStatus.baseMP + _EquipmentSlot.EquipmentBonusMP) * spellsvalue * MagicAttackMagnification;
                break;
            case AttackType.Physics:
                if (_EquipmentSlot.Weapon == null) finaldamage = _PlayerStatus.basePW;
                else finaldamage = (_PlayerStatus.basePW + _EquipmentSlot.EquipmentBonusPW) * _EquipmentSlot.Weapon.WeaponAP;
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

        finaldamage = damagevalue - _EquipmentSlot.Armor_DP;

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
