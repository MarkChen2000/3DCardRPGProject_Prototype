using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_InstantRestore_Ability", menuName = "Card/SpellsAbility/InstantRestore")]
public class Ability_InstantRestore : CardAbility
{
    public SpellsRestoreType RestoreType;
    public int RestoreAmount;

    public override void ActivateCardAbility(Vector3 mousePosition)
    {
        //Debug.Log("Activate InstantRestore " + name);

        GetScripts();
        SpawnActivateSpellsEffect();
        SpawnConsistEffect();

        _SpellsBattleManager.Spells_RestoreValue(RestoreType, RestoreAmount);
    }
}
