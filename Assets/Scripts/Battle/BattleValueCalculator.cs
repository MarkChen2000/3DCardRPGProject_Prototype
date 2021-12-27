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

    [Tooltip("This magnification will diverse magic and physics attack.")]
    public float MagicAttackMagnification = 1.5f;
    [Tooltip("When the physical damage is critical, multiply by this value.")]
    public float CriticalHitMagnification = 1.5f;

    private void Start()
    {
        PlayerStatusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
        EquipmentSlotCon = GameObject.Find("InventoryAndUIManager").GetComponent<EquipmentSlotController>();
    }
    // This function will do the player's and enemy's damage value calculation uniformly, All the damage formula can adjust at here.

    public Vector2 PlayerDamageCalculate(AttackType type, int spellsvalue) 
    {
    // Attacktype mean what is this attack type.
    // if the attack type is physics, the spellsvalue doesn't matter.
        float finaldamage = 0;
        int critic = 0; // 1 mean this hit is critical.
        switch ( type )
        {
            case AttackType.Magic:
                finaldamage = (PlayerStatusCon.baseMP + EquipmentSlotCon.EquipmentBonusMP) * spellsvalue * MagicAttackMagnification;
                break;
            case AttackType.Physics:

                // Caluculate Critical Hit!
                float magnification = 1;
                if (Random.Range(1, 100) <= EquipmentSlotCon.Weapon_CR) // when it is true, mean that this hit is critical!
                {
                    magnification = CriticalHitMagnification;
                    critic = 1;
                    Debug.Log("Critical Hit!");
                }

                if (EquipmentSlotCon.Weapon == null) finaldamage = Mathf.RoundToInt((PlayerStatusCon.basePW + EquipmentSlotCon.EquipmentBonusPW) * magnification);
                else finaldamage = Mathf.RoundToInt((PlayerStatusCon.basePW + EquipmentSlotCon.EquipmentBonusPW) * EquipmentSlotCon.Weapon.WeaponAP * magnification);
                break;
            case AttackType.Null:
                Debug.Log("This player's attack didnt have type!");
                break;
        }

        return new Vector2(critic,(int)Mathf.Round(finaldamage)); 
    }

    public int PlayerTakeDamageCalculate(int damagevalue) // call whenever player get attack;
    {
        float finaldamage = 0;

        finaldamage = Mathf.RoundToInt(damagevalue * (100 - EquipmentSlotCon.Armor_DP) / 100) ;

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
