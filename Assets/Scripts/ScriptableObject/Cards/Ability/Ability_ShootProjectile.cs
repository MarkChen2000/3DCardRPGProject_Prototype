using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New_ShootProjectil_Ability",menuName ="Card/SpellsAbility/ShootProjectile")]
public class Ability_ShootProjectile: CardAbility
{
    public GameObject ProjectilePrefab;

    public int Projectile_Damage = 1;
    public float Projectile_Speed = 10f;
    public float Projectile_LifeTime = 3f;

    public override void ActivateCardAbility(Vector3 mousePosition)
    {
        GetScripts();

        //Debug.Log("Activate Shooting Projectile "+name);

        SpawnActivateSpellsEffect();
        SpawnConsistEffect();

        GameObject projectile = Instantiate(ProjectilePrefab,Player_Trans.position,Quaternion.identity);

        Vector3 shoot_direction = (mousePosition - Player_Trans.position).normalized;
        float angle = Mathf.Atan2(shoot_direction.x, shoot_direction.z) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler( 0f, angle , 0f );

        ProjectileController pc = projectile.GetComponent<ProjectileController>();
        pc.Damage = Projectile_Damage;
        pc.speed = Projectile_Speed;
        pc.lifeTime = Projectile_LifeTime;
    }
}
