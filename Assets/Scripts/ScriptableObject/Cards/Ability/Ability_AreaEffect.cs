using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_AreaEffect_Ability", menuName = "Card/SpellsAbility/AreaEffect")]
public class Ability_AreaEffect : CardAbility
{
    public GameObject AreaEffectProjectilePrefab;

    public float Projectile_LifeTime = 3f;
    public int Projectile_Damage = 1;

    public override void ActivateCardAbility(Vector3 mousePosition)
    {
        GetScripts();

        //Debug.Log("Activate AreaEffect Spells: " + name);
        SpawnActivateSpellsEffect();
        SpawnConsistEffect();

        GameObject prefab = Instantiate(AreaEffectProjectilePrefab, mousePosition, Quaternion.identity);
        AreaEffectProjectileController con = prefab.GetComponent<AreaEffectProjectileController>();
        con.Damage = Projectile_Damage;
        con.lifeTime = Projectile_LifeTime;
    }

}
