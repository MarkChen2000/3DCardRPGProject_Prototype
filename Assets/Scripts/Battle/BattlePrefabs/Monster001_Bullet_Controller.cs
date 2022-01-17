using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster001_Bullet_Controller : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public int Damage;

    private Vector3 direction;
    private int type;
    
    private CharacterBasicAttackController CharacterBasicAttackCon;

    void Start()
    {
        this.CharacterBasicAttackCon = GameObject.Find("Player").GetComponent<CharacterBasicAttackController>();
    }
    public void Shoot(Vector3 direction)
    {
        this.direction = direction;
        KillSelf(lifeTime);
    }

    void Update()
    {
        this.gameObject.transform.Translate(this.direction * speed * Time.deltaTime);
    }

    private void KillSelf(float lifetime)
    {
        Destroy(this.gameObject,lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster") return;
        if (other.tag == "Projectile") return;

        if (other.tag == "Player")
        {
            CharacterBasicAttackCon.PlayerBeAttack(Damage);
        }
        KillSelf(0f);
    }
}
