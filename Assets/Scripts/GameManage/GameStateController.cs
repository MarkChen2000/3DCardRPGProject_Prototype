using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState 
{
    Prepare, // When player in village, prepare for next combat.
    Combat, // When player in the combat, he can not change the equipment and deck card.
    Tutorial // not doing anything now.
}

public class GameStateController : MonoBehaviour
{
    public GameState InitialGameState = GameState.Prepare;
    [HideInInspector]public GameState CurrentGameState = GameState.Prepare;

    public Transform CombatZoneSpawnPoint;
    public Transform PrepareZoneSpawnPoint;
    public GameObject Player;

    private EntireInventoryController EntireInvCon;
    private void Awake()
    {
        EntireInvCon = GameObject.Find("InventoryAndUIManager").GetComponent<EntireInventoryController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentGameState = InitialGameState;
    }

    public void EnterState(GameState enterstate)
    {
        switch ( enterstate )
        {
            case GameState.Combat:
                EnterCombat();
                break;
            case GameState.Prepare:
                EnterVillage();
                break;
        }

    }

    private void EnterCombat()
    {
        CurrentGameState = GameState.Combat;
        Player.transform.position = CombatZoneSpawnPoint.position;
        EntireInvCon.SwitchInventoryBarrierOnOff(true);
    }

    private void EnterVillage()
    {
        CurrentGameState = GameState.Prepare;
        Player.transform.position = PrepareZoneSpawnPoint.position;
        EntireInvCon.SwitchInventoryBarrierOnOff(false);
    }

}