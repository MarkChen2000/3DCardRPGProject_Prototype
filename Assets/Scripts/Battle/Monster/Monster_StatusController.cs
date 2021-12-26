using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Monster_StatusController : MonoBehaviour
{
    TextMeshProUGUI _text;
    private BattleValueCalculator BattleValueCal;
    private PlayerStatusLevelupSystem PlayerLevelupSystem;
    private PlayerStatusController PlayerStatusCon;
    public MonsterStatus _MonsterStatus;
    private int currentHP;

    public GameObject DeathEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _text = this.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

        currentHP = _MonsterStatus.max_hp;
        this.updateStatus(0);

        BattleValueCal = GameObject.Find("BattleManager").GetComponent<BattleValueCalculator>();
        PlayerLevelupSystem = GameObject.Find("PlayerManager").GetComponent<PlayerStatusLevelupSystem>();
        PlayerStatusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void beAttacked(int damage)
    {
        damage = BattleValueCal.EnemyTakeDamageCalculate(damage); 
        updateStatus(damage);

        if (currentHP <= 0)
        {
            MonsterDead();
        }

        Debug.Log(damage);
    }

    public void updateStatus(int number)
    {
        //_text.SetText("HP :" + (50 - number));
        _text.text = "HP :" + (currentHP - number);
        currentHP = currentHP - number;
    }

    private void MonsterDead()
    {
        PlayerLevelupSystem.GainExp(_MonsterStatus.Drop_Exp);
        PlayerStatusCon.GainMoney(_MonsterStatus.DropMoney);
        
        if ( DeathEffectPrefab!=null )
            Instantiate(DeathEffectPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

}
