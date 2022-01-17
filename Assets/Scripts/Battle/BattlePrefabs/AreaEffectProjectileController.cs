using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectProjectileController : MonoBehaviour
{
    BattleValueCalculator BattleValueCal;

    public GameObject CollisionEffectPrefab;
    [HideInInspector] public float lifeTime = 3f;
    [HideInInspector] public int Damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        BattleValueCal = GameObject.Find("BattleManager").GetComponent<BattleValueCalculator>();
        Destroy(gameObject,lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            other.GetComponent<Monster_StatusAndUIController>().beAttacked(this.BattleValueCal.PlayerDamageCalculate(AttackType.Magic, Damage));
            //other.SendMessage("beAttacked",this.battleValueCalculator.PlayerDamageCalculate(AttackType.Magic, Damage));
            if (CollisionEffectPrefab != null) Instantiate(CollisionEffectPrefab, transform.position, Quaternion.identity);
        }
    }

}
