using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_TemporaryBuff_Ability", menuName = "Card/SpellsAbility/TemporaryBuff")]
public class Ability_TemporaryBuff : CardAbility
{
    public SpellsBuffType BuffType;
    [Tooltip("Be cardful! Buff of PW and MP are multiply-type, RT and SP are plus/minus-type.")]
    public float BuffValue = 1f;
    public int BuffDuration = 60;

    public override void ActivateCardAbility(Vector3 direction)
    {
        GetScripts();
        SpawnActivateSpellsEffect();
        SpawnConsistEffect();

        _SpellsBattleManager.Spells_TemporaryBuff(BuffType, BuffValue, BuffDuration);
    }

}
