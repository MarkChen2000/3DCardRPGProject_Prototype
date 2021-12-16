using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_BehaviorController : MonoBehaviour
{
    private GameObject player;
    private Vector3 initPostition;
    private int speed;
    private float attackInterval;
    public GameObject bulletPrefab;
    private float currentTime;
    private float lastAttackTime;
    private int monsterType;
    private bool isAttack;
    private bool isOnCollision;
    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("Player");
        this.initPostition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        this.speed = 2;
        this.currentTime = Time.time;
        this.lastAttackTime = 0;
        this.isAttack = false;
        this.isOnCollision = false;
        if (this.gameObject.name.Contains("Monster001 1"))
        {
            this.monsterType = 1;
            this.attackInterval = 0.5f;
        }
        else if (this.gameObject.name.Contains("Monster001 2"))
        {
            this.monsterType = 2;
            this.attackInterval = 1f;
        }
        else if (this.gameObject.name.Contains("Monster001 3"))
        {
            this.speed = 4;
            this.monsterType = 3;
            this.attackInterval = 0.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.currentTime = Time.time;
        if (this.Detect())
        {
            this.gameObject.transform.LookAt(this.player.transform);
            this.gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (this.currentTime - this.lastAttackTime >= this.attackInterval)
            {
                if (this.monsterType == 1)
                {
                    this.Attack(this.monsterType);
                    this.lastAttackTime = this.currentTime;
                }
                else if (this.monsterType == 2 && !this.isAttack)
                {
                    this.Attack(this.monsterType);
                    this.lastAttackTime = this.currentTime;
                    this.isAttack = false;
                }
                else if (this.monsterType == 3)
                {
                    this.Attack(this.monsterType);
                    this.lastAttackTime = this.currentTime;
                }
            }

        }
    }

    private bool Detect()
    {
        if (Vector3.Distance(this.initPostition, this.transform.position) <= 200f)
        {
            if (Vector3.Distance(this.transform.position, this.player.transform.position) <= 20f && !isOnCollision)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }

    private void Attack(int type)
    {
        if (type == 1)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            bullet.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(type, Vector3.forward);
        }
        else if (type == 2)
        {
            GameObject bullet_F = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            GameObject bullet_B = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            GameObject bullet_R = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            GameObject bullet_L = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            bullet_F.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(type, Vector3.forward);
            bullet_B.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(type, Vector3.back);
            bullet_L.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(type, Vector3.left);
            bullet_R.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(type, Vector3.right);
        }
        else if (type == 3)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            bullet.transform.GetComponent<Monster001_Bullet_Controller>().Shoot(type, Vector3.forward);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.isOnCollision = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.isOnCollision = false;
        }
    }

}
