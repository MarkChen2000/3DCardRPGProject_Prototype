using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Monster_StatusController : MonoBehaviour
{
    TextMeshProUGUI _text;
    private BattleValueCalculator BattleValueCal;
    public MonsterStatus _MonsterStatus;
    private int currentHP;

    // Start is called before the first frame update
    void Start()
    {
        _text = this.transform.parent.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        Debug.Log(_text.text.ToString());

        if ( _MonsterStatus==null )
        {
            _MonsterStatus = Resources.Load<MonsterStatus>("Monster/Monster001");
        }
        currentHP = _MonsterStatus.max_hp;

        BattleValueCal = GameObject.Find("BattleManager").GetComponent<BattleValueCalculator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void beAttacked(int damage)
    {
        damage = BattleValueCal.EnemyTakeDamageCalculate(damage); 
        updateStatus(damage);
        Debug.Log(damage);
    }

    public void updateStatus(int number)
    {
        //_text.SetText("HP :" + (50 - number));
        _text.text = "HP :" + (currentHP - number);
        currentHP = currentHP - number;
    }
}
