using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState 
{
    MainMenu, // After game start, will show the mainmenu first.
    Prepare, // When player in village, prepare for next combat.
    Combat, // When player in the combat, he can not change the equipment and deck card.
    Tutorial // not doing anything now.
}

public class GameStateController : MonoBehaviour
{
    public GameState InitialGameState;
    [HideInInspector]public GameState CurrentGameState;

    public Transform CombatZoneSpawnPoint;
    public Transform PrepareZoneSpawnPoint;
    public GameObject Player;

    PlayerStatusController PlayerStatusCon;
    EntireInventoryController EntireInvCon;
    CardBattleController CardBattleCon;
    MenuUIManager _MenuUIManager;
    BattleUIManager _BattleUIManager;
    CharacterMovementController PlayerMovementCon;
    EntireInventoryController InvCon;
    BgmManager BgmCon;

    private void Awake()
    {
        PlayerStatusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
        EntireInvCon = GameObject.Find("InventoryAndUIManager").GetComponent<EntireInventoryController>();
        CardBattleCon = GameObject.Find("BattleUI").GetComponent<CardBattleController>();
        _MenuUIManager = FindObjectOfType<MenuUIManager>();
        _BattleUIManager = FindObjectOfType<BattleUIManager>();
        PlayerMovementCon = FindObjectOfType<CharacterMovementController>();
        InvCon = FindObjectOfType<EntireInventoryController>();
	    BgmCon = GameObject.Find("BgmManager").GetComponent<BgmManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentGameState = InitialGameState;
        EnterState(CurrentGameState);
    }

    public void EnterState(GameState enterstate)
    {
        switch ( enterstate )
        {
            case GameState.MainMenu:
                EnterMainMenu();
                break;
            case GameState.Combat:
                EnterCombat();
                break;
            case GameState.Prepare:
                EnterVillage();
                break;
        }

    }

    void EnterMainMenu()
    {
        CurrentGameState = GameState.MainMenu;

        _BattleUIManager.SwitchUpStatusUIOnOff(false);
        _MenuUIManager.SwitchMainMenuOnOff(true);

        PlayerMovementCon.Can_Control = false;

        InvCon.Can_TurnOnInv = false;

        BgmCon.BgmPlayMainMenu();
    }

    public void FirstEnterVillage() // enter village from main menu state.
    {
        CurrentGameState = GameState.Prepare;

        _BattleUIManager.SwitchUpStatusUIOnOff(true);
        _MenuUIManager.SwitchMainMenuOnOff(false);

        PlayerMovementCon.Can_Control = true;

        InvCon.Can_TurnOnInv = true;

        BgmCon.BgmPlayPrepare();
    }

    void EnterVillage()
    {
        CurrentGameState = GameState.Prepare;

        Player.transform.position = PrepareZoneSpawnPoint.position;

        EntireInvCon.SwitchInventoryBarrierOnOff(false);

        StopCoroutine(PlayerStatusCon.RestoringMana());

        PlayerStatusCon.RefillAllStatusValue();

        CardBattleCon.LeaveCombat();
	    BgmCon.BgmPlayPrepare();
    }

    void EnterCombat()
    {
        CurrentGameState = GameState.Combat;

        Player.transform.position = CombatZoneSpawnPoint.position;

        EntireInvCon.SwitchInventoryBarrierOnOff(true);

        StopCoroutine(PlayerStatusCon.RestoringMana()); // just in case the coroutine doesn't stop.
        StartCoroutine(PlayerStatusCon.RestoringMana());

        PlayerStatusCon.RefillAllStatusValue();

        CardBattleCon.EnterCombatInitialize();
	    BgmCon.BgmPlayCombat();
    }


}
