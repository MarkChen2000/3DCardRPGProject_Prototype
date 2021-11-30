using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private GameObject monster;
    private CardData card;
    private PlayerStatus playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = Resources.Load<PlayerStatus>("Player/MainPlayer");
        this.monster = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void currentTarget(GameObject gameObject)
    {
        this.monster = gameObject;
        //Debug.Log(this.monster.ToString());
    }

    public void executeCard(CardData card)
    {
        Debug.Log(this.monster);
        if (monster != null)
        {
            this.card.ExecuteCardFunction(this.card.CardName, this.monster, this.playerStatus);
            GameObject.Find("BattleUI").GetComponent<CradBattleUIController>().updateStatus(this.card);
        }
    }

    public void currentCard(CardData card)
    {
        this.card = card;
    }
}
