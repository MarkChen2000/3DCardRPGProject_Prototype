using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //private GameObject monster;
    private CardData card;

    public void executeCard(CardData card, Vector3 mousePosition)
    {
        //this.card.ExecuteCardFunction(this.card.CardName, direction, this.playerStatusCon, currentPrefab);
        this.card.ActivateCardAbility(mousePosition);
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
