using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellsRestoreType
{
    HP,Mana
}

public enum SpellsBuffType
{
    PW,MP,RT,SP
}

public class CardAbility : ScriptableObject
{
    protected Transform Player_Trans;
    protected SpellsBattleManager _SpellsBattleManager;
    protected PlayerSpellsEffectController PlayerSpellsEffectCon;
    protected BattleUIManager _BattleUIManager;

    public bool Using_RangeDetect = false; // the spells is range type
    public float ActivateRange = 10f;

    public GameObject ActivateEffectPrefab;
    public float ActivateEffectLifeTime = 1f;
    public GameObject ConsistEffectPrefab;
    public float ConsistEffectLifetime = 3f;

    void OnEnable()
    {
        // Because OnEnable will be called even before game started ( maybe called when the SO is start using )
        // Make it have some sort of misreference and wrong (or error) instance reference.
        // As a result, before I figure out how to slove the problem, I have to make them GetComponent those instance
        // whenever the spells get activate. This method can slove the problem but also cost more performance.
        /*Player_Trans = GameObject.Find("Player").GetComponent<Transform>();
        PlayerStatus_Con = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();*/

        // Debug.Log("Enable" + name);
    }

    protected void GetScripts() // all the inherited abilities have to use this function to access the scripts instances.
                                // For reducing the performance cost of getting components, using SpellsBattleManager to access component together. 
    {
        _SpellsBattleManager = FindObjectOfType<SpellsBattleManager>();
        Player_Trans = _SpellsBattleManager.Player.transform;
        PlayerSpellsEffectCon = _SpellsBattleManager.PlayerSpellsEffectCon;
        _BattleUIManager = _SpellsBattleManager._BattleUIManager;
    }

    public bool CheckInActivateRange(Vector3 mousePosition)
    {
        if ( Using_RangeDetect )
        {
            GetScripts();
            if (Vector3.Distance(mousePosition, Player_Trans.position) > ActivateRange)
            {
                _BattleUIManager.SpawnHandCardPopupInfo("Out of card activate range!", mousePosition);
                return false;
            }
        }
        return true;
    }

    public virtual void ActivateCardAbility(Vector3 mousePosition)
    {
        GetScripts();

        // these function should be override.
    }

    protected void SpawnActivateSpellsEffect()
    {
        if (ActivateEffectPrefab != null)
        {
            PlayerSpellsEffectCon.SpawnSpellsActivateEffect(ActivateEffectPrefab, ActivateEffectLifeTime);
        }
    }

    protected void SpawnConsistEffect()
    {
        if (ConsistEffectPrefab != null)
            PlayerSpellsEffectCon.SpawnSpellsConsistEffect(ConsistEffectPrefab, ConsistEffectLifetime);
    }

}
