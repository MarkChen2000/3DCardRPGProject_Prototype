using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool isOnCollision;

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("Player");
        this.initPostition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        this.currentTime = Time.time;
        this.isAttack = false;
        this.isOnCollision = false;
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

    private void Attack()
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
