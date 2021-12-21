using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster001_Bullet_Controller : MonoBehaviour
{
    private float speed;
    private Vector3 direction;
    private int type;
    private float lifeTime;
    private MonsterStatus monsterStatus001;
    private MonsterStatus monsterStatus002;
    private MonsterStatus monsterStatus003;
    private BattleValueCalculator battleValueCalculator;
    private PlayerStatusController playerStatusController;

    void Start()
    {
        battleValueCalculator = GameObject.Find("BattleManager").GetComponent<BattleValueCalculator>();
        this.monsterStatus001 = Resources.Load<MonsterStatus>("Assets/Prefabs/Monsters/Monster001 1");
        this.monsterStatus002 = Resources.Load<MonsterStatus>("Assets/Prefabs/Monsters/Monster001 2");
        this.monsterStatus003 = Resources.Load<MonsterStatus>("Assets/Prefabs/Monsters/Monster001 3");
        this.playerStatusController = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
    }
    public void Shoot(int type, Vector3 direction)
    {
        this.type = type;
        this.direction = direction;
        if (type == 1)
        {
            this.speed = 9;
            this.lifeTime = 3;
        }
        else if (type == 2)
        {
            this.speed = 8;
            this.lifeTime = 3;
        }
        else if (type == 3)
        {
            this.speed = 10;
            this.lifeTime = 4;
        }
        Invoke("KillSelf", lifeTime);
    }

    void Update()
    {
        this.gameObject.transform.Translate(this.direction * speed * Time.deltaTime);
    }

    private void KillSelf()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (this.type == 1)
            {
                
                this.playerStatusController.UpdateStatus("currentHP", -this.battleValueCalculator.PlayerTakeDamageCalculate(25));
            }
            else if (this.type == 2)
            {
                this.playerStatusController.UpdateStatus("currentHP", -this.battleValueCalculator.PlayerTakeDamageCalculate(50));
            }
            else if (this.type == 3)
            {
                this.playerStatusController.UpdateStatus("currentHP", -this.battleValueCalculator.PlayerTakeDamageCalculate(25));
            }
            this.KillSelf();
        }
    }
}
