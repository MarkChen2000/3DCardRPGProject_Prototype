using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsBattleManager : MonoBehaviour
{
    public GameObject Player;

    // These scripts have to access instance for cardability, so they have to be public.
    [HideInInspector] public PlayerSpellsEffectController PlayerSpellsEffectCon;
    [HideInInspector] public PlayerStatusController PlayerStatusCon;
    [HideInInspector] public BattleUIManager _BattleUIManager;

    void Awake()
    {
        if ( Player==null) Player = GameObject.Find("Player");
        PlayerSpellsEffectCon = Player.GetComponent<PlayerSpellsEffectController>();
        PlayerStatusCon = FindObjectOfType<PlayerStatusController>();
        _BattleUIManager = FindObjectOfType<BattleUIManager>();
    }

    public IEnumerator RestoringMana()
    {
        while (true) // every frame check the mana is leas then max value.
        {
            while (PlayerStatusCon.currentMana < PlayerStatusCon.currentMaxMana)
            {
                yield return new WaitForSeconds(PlayerStatusCon.currentManaRT); // after wait for RT then restore 1 mana.
                PlayerStatusCon.RestoreMana(1);
            }
            yield return null;
        }
    }

    public void Spells_RestoreValue(SpellsRestoreType type, int restore_amount)
    {
        switch (type)
        {
            case SpellsRestoreType.HP:
                PlayerStatusCon.RestoreHP(PlayerStatusCon.currentMP * restore_amount);
                break;
            case SpellsRestoreType.Mana:
                PlayerStatusCon.RestoreMana(restore_amount);
                break;
        }
    }

    public void ResetAllTemporaryBuff()
    {
        // Warning! This will stop all the coroutine on this script! Although it only have tmeporary buff for now,
        // but becareful the coroutines that be added later will also be stoped!
        Debug.Log("Stop all coroutines in SpellsBattleController!"); 
        StopAllCoroutines();

        PlayerStatusCon.ResetAllStatusValue();
    }

    public void Spells_TemporaryBuff(SpellsBuffType type, float buffvalue, float duration)
    {
        int disparity = 0;
        switch (type)
        {
            case SpellsBuffType.PW:
                disparity = Mathf.RoundToInt(PlayerStatusCon.currentPW * buffvalue);
                PlayerStatusCon.currentPW += disparity;
                break;
            case SpellsBuffType.MP:
                disparity = Mathf.RoundToInt(PlayerStatusCon.currentMP * buffvalue);
                PlayerStatusCon.currentMP += disparity;
                break;
            case SpellsBuffType.RT:
                disparity = (int)buffvalue;
                PlayerStatusCon.currentManaRT -= disparity;
                break;
            case SpellsBuffType.SP:
                disparity = (int)buffvalue;
                PlayerStatusCon.currentSP += disparity;
                break;
        }
        Debug.Log("Buff " + type.ToString() + " by " + disparity);
        StartCoroutine(WaitforBuffEnd(type, duration, disparity));
    }

    IEnumerator WaitforBuffEnd(SpellsBuffType type, float duraion, int disparity)
    {
        yield return new WaitForSeconds(duraion);
        switch (type)
        {
            case SpellsBuffType.PW:
                PlayerStatusCon.currentPW -= disparity;
                break;
            case SpellsBuffType.MP:
                PlayerStatusCon.currentMP -= disparity;
                break;
            case SpellsBuffType.RT:
                PlayerStatusCon.currentManaRT += disparity;
                break;
            case SpellsBuffType.SP:
                PlayerStatusCon.currentSP -= disparity;
                break;
        }
        Debug.Log("Return buff " + type.ToString() + " by " + disparity);
    }

    // This script used to be the battle manager for cards.
    /*public void executeCard(CardData card, Vector3 mousePosition)
    {
        card.ActivateCardAbility(mousePosition);
        GameObject.Find("BattleUI").GetComponent<HandCardBattleController>().updateStatus(card);
    }

    public void DrawCard()
    {
        GameObject.Find("BattleUI").GetComponent<HandCardBattleController>().DrawCard();
    }*/
}
