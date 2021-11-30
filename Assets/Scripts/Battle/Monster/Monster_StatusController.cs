using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Monster_StatusController : MonoBehaviour
{
    TextMeshProUGUI _text;
    private int currentHP;

    // Start is called before the first frame update
    void Start()
    {
        _text = this.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        Debug.Log(_text.text.ToString());
        currentHP = Resources.Load<MonsterStatus>("Monster/Monster001").max_hp;
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
