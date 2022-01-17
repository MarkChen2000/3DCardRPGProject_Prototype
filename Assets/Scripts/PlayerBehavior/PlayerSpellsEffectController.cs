using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellsEffectController : MonoBehaviour
{

    public Transform Trans_SpellsEffectSpot;

    void Awake()
    {
        
    }

    public void ClearAllSpellsConsistEffect()
    {
        for (int i = 0; i < Trans_SpellsEffectSpot.childCount ; i++)
        {
            Destroy(Trans_SpellsEffectSpot.GetChild(i).gameObject);
        }
    }

    public void SpawnSpellsActivateEffect(GameObject effectprefab, float duration)
    {
        GameObject prefab = Instantiate(effectprefab, Trans_SpellsEffectSpot.position, Quaternion.identity);
        Destroy(prefab, duration);
    }

    public void SpawnSpellsConsistEffect(GameObject effectprefab, float duration)
    {
        GameObject prefab = Instantiate(effectprefab, Trans_SpellsEffectSpot);
        Destroy(prefab, duration);
    }


}
