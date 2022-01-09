using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellsRestoreType
{
    HP,Mana
}

[CreateAssetMenu(fileName ="CardAbilityBase",menuName ="Card/BaseAbility")]
public class CardAbility : ScriptableObject
{
    protected Transform Player_Trans;
    protected PlayerStatusController PlayerStatus_Con;

    public GameObject SpellsActivateEffectPrefab;
    public float ActivateEffectLifeTime = 3f;


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

    public virtual void ActivateCardAbility(Vector3 direction)
    {
        Player_Trans = GameObject.Find("Player").transform;
        PlayerStatus_Con = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();

        GameObject prefab;
        if (SpellsActivateEffectPrefab != null)
        {
            prefab = Instantiate(SpellsActivateEffectPrefab, Player_Trans.position, Quaternion.identity);
            prefab.GetComponent<PrefabSelfDestroyController>().lifeTime = ActivateEffectLifeTime;
        }
    }
}
