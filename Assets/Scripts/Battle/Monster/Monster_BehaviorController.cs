using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Monster_BehaviorController : MonoBehaviour
{
    private GameObject player;
    private Vector3 initPostition;

    public int speed;
    public float attackInterval;
    public int monsterType;
    public GameObject bulletPrefab;

    private float currentTime;
    private float lastAttackTime;
    private bool isAttack;
    //private bool isOnCollision;
    public int current_hp_percentage;

    private IEnumerator rotateFire;

    private GameState currentGameState;

    [HideInInspector] public bool OnAttackMode = false;

    public GameObject slimePrefab;
    public GameObject turtlePrefab;
    public GameObject stonePrefab;

    private bool summon_Controller;
    private bool healthRestorIsAtivated;

    private Monster_StatusAndUIController monster_StatusAndUIController;

    public List<GameObject> SummoningPointList;

    public List<GameObject> BulletPrefabList;

    // Start is called before the first frame update
    void Start()
    {

        this.player = GameObject.Find("Player");
        this.initPostition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        this.currentTime = Time.time;
        this.isAttack = false;
        //this.isOnCollision = false;
        this.current_hp_percentage = 100;
        this.rotateFire = RotateFire();
        this.currentGameState = GameObject.Find("GameManager").GetComponent<GameStateController>().CurrentGameState;
        this.summon_Controller = true;
        this.healthRestorIsAtivated = false;
        this.monster_StatusAndUIController = this.gameObject.GetComponent<Monster_StatusAndUIController>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.currentTime = Time.time;
        if (this.Detect())
        {
            OnAttackMode = true;
            this.gameObject.transform.LookAt(this.player.transform);
            if (Vector3.Distance(this.transform.position, this.player.transform.position) >= 3f)
            {
                //this.gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
                this.gameObject.GetComponent<Rigidbody>().velocity = this.transform.forward * speed;
            }
            if (this.currentTime - this.lastAttackTime >= this.attackInterval)
            {
                if (this.monsterType == 1)
                {
                    this.Attack();
                    this.lastAttackTime = this.currentTime;
                }
                else if (this.monsterType == 2 && !this.isAttack)
                {
                    this.Attack();
                    this.lastAttackTime = this.currentTime;
                    this.isAttack = false;
                }
                else if (this.monsterType == 3)
                {
                    this.Attack();
                    this.lastAttackTime = this.currentTime;
                }
                else if (this.monsterType == 4)
                {
                    this.Attack(this.current_hp_percentage);
                    this.lastAttackTime = this.currentTime;
                }
                else if (this.monsterType == 5)
                {
                    this.Attack(this.current_hp_percentage);
                    this.lastAttackTime = this.currentTime;
                }
            }
            while (this.summon_Controller && this.monsterType == 4)
            {
                InvokeRepeating("Summon_servant", 0f, 12f);
                Debug.Log("Summoning!");
                this.summon_Controller = false;
            }
        }
        else OnAttackMode = false;
    }

    private bool Detect()
    {
        if (Vector3.Distance(this.initPostition, this.transform.position) <= 40f)
        {
            StopCoroutine(RestoreHealthTimer());
            StopCoroutine(RestoreHealth());
            //if (Vector3.Distance(this.transform.position, this.player.transform.position) <= 15f && !isOnCollision)
            if (Vector3.Distance(this.transform.position, this.player.transform.position) <= 25f)
            {
                if (this.monsterType < 4)
                {
                    StopCoroutine(KillSelfCounter());
                }
                
                return true;
            }
            else
            {
                if (this.monsterType < 4 && !this.healthRestorIsAtivated)
                {
                    StartCoroutine(KillSelfCounter());
                    this.healthRestorIsAtivated = true;
                }
                
                return false;
            }
        }
        else
        {
            if (this.monsterType >= 4)
            {
                StartCoroutine(RestoreHealthTimer());
            }
            
            return false;
        }
    }

    private void Attack(int hp_left = 0)
    {
        if (monsterType == 1)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            bullet.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.forward);
        }
        else if (monsterType == 2)
        {
            GameObject bullet_F = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            GameObject bullet_B = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            GameObject bullet_R = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            GameObject bullet_L = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            bullet_F.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.forward);
            bullet_B.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.back);
            bullet_L.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.left);
            bullet_R.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.right);
        }
        else if (monsterType == 3)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            bullet.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.forward);
        }
        else if (monsterType == 4)
        {
            System.Random rnd = new System.Random();
            if (hp_left <= 25)
            {
                this.speed = 7;
                this.attackInterval = 0.5f;
                float temp = rnd.Next(1, 100);
                if (temp <= 10f)
                {
                    RoundFire();
                    ShotGun();
                }
                else if (temp <= 25f)
                {
                    RoundFire();
                }
                else if (temp <= 50f)
                {
                    ShotGun();
                    StartCoroutine(rotateFire);
                }
                else
                {
                    ShotGun();
                }
            }
            else if (hp_left <= 50)
            {
                this.speed = 6;
                this.attackInterval = 1f;
                float temp = rnd.Next(1, 100);
                if (temp <= 10f)
                {
                    RoundFire();
                }
                else if (temp <= 25f)
                {
                    StartCoroutine(rotateFire);
                }
                else if (temp <= 75)
                {
                    ShotGun();
                }
                else
                {
                    GameObject bullet_F = Instantiate(this.BulletPrefabList[1], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                    GameObject bullet_B = Instantiate(this.BulletPrefabList[1], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                    GameObject bullet_R = Instantiate(this.BulletPrefabList[1], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                    GameObject bullet_L = Instantiate(this.BulletPrefabList[1], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                    bullet_F.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.forward);
                    bullet_B.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.back);
                    bullet_L.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.left);
                    bullet_R.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.right);
                }
            }
            else if (hp_left <= 75)
            {
                this.speed = 5;
                this.attackInterval = 1.5f;
                float temp = rnd.Next(1, 100);
                if (temp <= 5f)
                {
                    RoundFire();
                }
                else if (temp <= 75f)
                {
                    ShotGun();
                }
                else
                {
                    GameObject bullet_F = Instantiate(this.BulletPrefabList[1], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                    GameObject bullet_B = Instantiate(this.BulletPrefabList[1], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                    GameObject bullet_R = Instantiate(this.BulletPrefabList[1], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                    GameObject bullet_L = Instantiate(this.BulletPrefabList[1], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                    bullet_F.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.forward);
                    bullet_B.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.back);
                    bullet_L.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.left);
                    bullet_R.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.right);
                }
            }
            else
            {
                this.attackInterval = 1.5f;
                float temp = rnd.Next(1, 100);
                if (temp <= 50f)
                {
                    ShotGun();
                }
                else
                {
                    GameObject bullet_F = Instantiate(this.BulletPrefabList[1], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                    GameObject bullet_B = Instantiate(this.BulletPrefabList[1], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                    GameObject bullet_R = Instantiate(this.BulletPrefabList[1], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                    GameObject bullet_L = Instantiate(this.BulletPrefabList[1], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                    bullet_F.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.forward);
                    bullet_B.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.back);
                    bullet_L.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.left);
                    bullet_R.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.right);
                }
            }
        }
        else if (this.monsterType == 5)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            bullet.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(this.transform.forward);
            GameObject bullet_F = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            GameObject bullet_B = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            GameObject bullet_R = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            GameObject bullet_L = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            bullet_F.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.forward);
            bullet_B.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.back);
            bullet_L.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.left);
            bullet_R.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(Vector3.right);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            //this.isOnCollision = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            //this.isOnCollision = false;
        }
    }

    private void RoundFire()
    {
        for (int i = 0; i < 36; i++)
        {
            GameObject bullet = Instantiate(this.BulletPrefabList[1], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.Euler(0, i * 10, 0));
            bullet.GetComponent<Monster001_Bullet_Controller>().speed = 16;
            bullet.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(bullet.transform.forward);
        }
    }

    private void ShotGun()
    {
        GameObject bullet_A = Instantiate(this.BulletPrefabList[2], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.Euler(0, 30, 0));
        GameObject bullet_B = Instantiate(this.BulletPrefabList[2], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.Euler(0, 0, 0));
        GameObject bullet_C = Instantiate(this.BulletPrefabList[2], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.Euler(0, 330, 0));
        GameObject bullet_D = Instantiate(this.BulletPrefabList[2], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.Euler(0, 210, 0));
        GameObject bullet_E = Instantiate(this.BulletPrefabList[2], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.Euler(0, 180, 0));
        GameObject bullet_F = Instantiate(this.BulletPrefabList[2], new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.Euler(0, 150, 0));
        bullet_A.GetComponent<Monster001_Bullet_Controller>().speed = 12;
        bullet_A.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(bullet_A.transform.forward);
        bullet_B.GetComponent<Monster001_Bullet_Controller>().speed = 12;
        bullet_B.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(bullet_B.transform.forward);
        bullet_C.GetComponent<Monster001_Bullet_Controller>().speed = 12;
        bullet_C.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(bullet_C.transform.forward);
        bullet_D.GetComponent<Monster001_Bullet_Controller>().speed = 12;
        bullet_D.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(bullet_D.transform.forward);
        bullet_E.GetComponent<Monster001_Bullet_Controller>().speed = 12;
        bullet_E.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(bullet_E.transform.forward);
        bullet_F.GetComponent<Monster001_Bullet_Controller>().speed = 12;
        bullet_F.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(bullet_F.transform.forward);
    }
    private IEnumerator RotateFire()//参数为发射波数与子弹生成点
    {
        Vector3 bulletDir = this.transform.up;//发射方向
        Quaternion rotateQuate = Quaternion.AngleAxis(10, Vector3.forward);//使用四元数制造绕Z轴旋转10度的旋转
        for (int i = 0; i < 1; i++)    //发射波数
        {
            for (int j = 0; j < 36; j++)
            {
                GameObject bullet = Instantiate(this.BulletPrefabList[0]);   //生成子弹
                bullet.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
                bullet.transform.rotation = Quaternion.Euler(bulletDir);
                bullet.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(bullet.transform.forward);
                bulletDir = rotateQuate * bulletDir; //让发射方向旋转10度，到达下一个发射方向
            }
            yield return new WaitForSeconds(0.05f); //协程延时，0.5秒进行下一波发射
        }
        StopCoroutine(rotateFire);
        yield return null;
    }

    private void Summon_servant()
    {
        if (this.current_hp_percentage > 75)
        {
            GameObject monster_A = Instantiate(slimePrefab, this.SummoningPointList[0].transform);
            GameObject monster_B = Instantiate(slimePrefab, this.SummoningPointList[2].transform);
            GameObject monster_C = Instantiate(slimePrefab, this.SummoningPointList[4].transform);
            monster_A.transform.parent = null;
            monster_B.transform.parent = null;
            monster_C.transform.parent = null;
        }
        else if (this.current_hp_percentage > 50)
        {
            GameObject monster_A = Instantiate(slimePrefab, this.SummoningPointList[0].transform);
            GameObject monster_B = Instantiate(slimePrefab, this.SummoningPointList[2].transform);
            GameObject monster_C = Instantiate(slimePrefab, this.SummoningPointList[4].transform);
            monster_A.transform.parent = null;
            monster_B.transform.parent = null;
            monster_C.transform.parent = null;

            GameObject monster_D = Instantiate(turtlePrefab, this.SummoningPointList[1].transform);
            monster_D.transform.parent = null;
        }
        else
        {
            GameObject monster_A = Instantiate(turtlePrefab, this.SummoningPointList[1].transform);
            GameObject monster_B = Instantiate(turtlePrefab, this.SummoningPointList[3].transform);
            GameObject monster_C = Instantiate(turtlePrefab, this.SummoningPointList[4].transform);
            monster_A.transform.parent = null;
            monster_B.transform.parent = null;
            monster_C.transform.parent = null;
        }
    }

    private IEnumerator KillSelfCounter()
    {
        yield return new WaitForSeconds(10f);
        this.monster_StatusAndUIController.Despawn();
    }

    private IEnumerator RestoreHealthTimer()
    {
        yield return new WaitForSeconds(10f);
        StartCoroutine(RestoreHealth());
    }

    private IEnumerator RestoreHealth()
    {
        while (this.current_hp_percentage <= 100)
        {
            this.monster_StatusAndUIController.updateStatus(-500);
            yield return new WaitForSeconds(0.5f);
        }

    }
}
