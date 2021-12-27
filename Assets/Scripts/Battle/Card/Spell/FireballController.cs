using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    private Vector3 direction;
    private PlayerStatusController PlayerStatus;
    private float speed;
    private BattleValueCalculator battleValueCalculator;
    private float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        this.speed = 12f;
        this.gameObject.transform.position = new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, GameObject.Find("Player").transform.position.z);
        this.gameObject.transform.LookAt(new Vector3(this.direction.x, GameObject.Find("Player").transform.position.y, this.direction.z));
        this.battleValueCalculator = GameObject.Find("BattleManager").GetComponent<BattleValueCalculator>();
        this.lifeTime = 2f;
        Invoke("KillSelf", this.lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //this.gameObject.transform.LookAt(this.direction);
    }

    public void Shoot(Vector3 direction, PlayerStatusController playerStatus)
    {
        this.direction = direction;
        Debug.Log(this.direction);
        this.PlayerStatus = playerStatus;
        //this.gameObject.transform.LookAt(new Vector3(-this.direction.x, GameObject.Find("Player").transform.position.y, -this.direction.z));
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Monster")
        {
            other.SendMessage("beAttacked",this.battleValueCalculator.PlayerDamageCalculate(AttackType.Magic, 6));
            KillSelf();
        }
    }

    private void KillSelf() {
        Destroy(this.gameObject);
    }
}
