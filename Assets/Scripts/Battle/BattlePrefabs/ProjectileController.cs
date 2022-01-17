using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    /*private Vector3 direction;
    private PlayerStatusController PlayerStatus;*/

    BattleValueCalculator battleValueCalculator;

    public GameObject CollisionEffectPrefab;
    [HideInInspector] public float speed = 5f;
    [HideInInspector] public float lifeTime = 3f;
    [HideInInspector] public int Damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        /*this.speed = 12f;
        this.gameObject.transform.position = new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, GameObject.Find("Player").transform.position.z);
        this.gameObject.transform.LookAt(new Vector3(this.direction.x, GameObject.Find("Player").transform.position.y, this.direction.z));
        this.battleValueCalculator = GameObject.Find("BattleManager").GetComponent<BattleValueCalculator>();
        this.lifeTime = 2f;*/
        battleValueCalculator = GameObject.Find("BattleManager").GetComponent<BattleValueCalculator>();
        Destroy(gameObject,lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.gameObject.transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
        //this.gameObject.transform.LookAt(this.direction);
    }

    /*public void Shoot(Vector3 direction, PlayerStatusController playerStatus)
    {
        this.direction = direction;
        Debug.Log(this.direction);
        this.PlayerStatus = playerStatus;
        //this.gameObject.transform.LookAt(new Vector3(-this.direction.x, GameObject.Find("Player").transform.position.y, -this.direction.z));
    }*/

    void OnTriggerEnter(Collider other) {

        //Debug.Log(name + "hit" + other.name);
        if (other.tag == "Player") return;
        if (other.tag == "Projectile") return;
        if (other.tag == "Monster")
        {
            other.GetComponent<Monster_StatusAndUIController>().beAttacked(this.battleValueCalculator.PlayerDamageCalculate(AttackType.Magic, Damage));
            //other.SendMessage("beAttacked",this.battleValueCalculator.PlayerDamageCalculate(AttackType.Magic, Damage));
            if (CollisionEffectPrefab!=null) Instantiate(CollisionEffectPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}
