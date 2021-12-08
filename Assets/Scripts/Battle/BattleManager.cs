using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //private GameObject monster;
    private CardData card;
    private PlayerStatus playerStatus;
    private GameObject currentPrefab;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = Resources.Load<PlayerStatus>("Player/MainPlayer");
        //this.monster = null;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void currentTarget(GameObject gameObject)
    {
        //this.monster = gameObject;
    }

    public void executeCard(CardData card, Vector3 mousePosition)
    {
        this.card.ExecuteCardFunction(this.card.CardName, mousePosition, this.playerStatus, currentPrefab);
        GameObject.Find("BattleUI").GetComponent<CardBattleController>().updateStatus(card);
    }

    public void currentCard(CardData card)
    {
        this.card = card;
    }

    public void DrawCard()
    {
        GameObject.Find("BattleUI").GetComponent<CardBattleController>().DrawCard();
    }
}
