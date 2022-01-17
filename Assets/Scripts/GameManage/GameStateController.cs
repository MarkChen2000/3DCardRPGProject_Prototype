using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState 
{
    MainMenu, // After game start, will show the mainmenu first.
    Prepare, // When player in village, prepare for next combat.
    Combat, // When player in the combat, he can not change the equipment and deck card.
    Tutorial, // not doing anything now.
    RestartMenu, // restart mean player was defeat.
    BossFight,
    GameOverMenu // Gameover mean victory.
}

public class GameStateController : MonoBehaviour
{
    public GameState InitialGameState;
    [HideInInspector]public GameState CurrentGameState;

    public Transform CombatZoneSpawnPoint;
    public Transform PrepareZoneSpawnPoint;
    public Transform RespawnPoint;
    public Transform BossZoneTeleportPoint;
    public GameObject Player;

    PlayerStatusController PlayerStatusCon;
    EntireInventoryController EntireInvCon;
    HandCardBattleController HandCardBattleCon;
    MenuUIManager _MenuUIManager;
    BattleUIManager _BattleUIManager;
    CharacterMovementController PlayerMovementCon;
    CharacterBasicAttackController PlayerBasicAttackCon;
    SpellsBattleManager _SpellsBattleManager;
    PlayerSpellsEffectController PlayerSpellsEffectCon;
    BgmManager BgmCon;

    private void Awake()
    {
        PlayerStatusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
        EntireInvCon = GameObject.Find("InventoryAndUIManager").GetComponent<EntireInventoryController>();
        HandCardBattleCon = GameObject.Find("BattleUI").GetComponent<HandCardBattleController>();
        _MenuUIManager = FindObjectOfType<MenuUIManager>();
        _BattleUIManager = FindObjectOfType<BattleUIManager>();
        PlayerMovementCon = FindObjectOfType<CharacterMovementController>();
        PlayerBasicAttackCon = FindObjectOfType<CharacterBasicAttackController>();
        _SpellsBattleManager = FindObjectOfType<SpellsBattleManager>();
        PlayerSpellsEffectCon = GameObject.Find("Player").GetComponent<PlayerSpellsEffectController>();
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
            case GameState.RestartMenu:
                EnterRestartMenu();
                break;
            case GameState.BossFight:
                EnterBossFight();
                break;
            case GameState.GameOverMenu:
                EnterGameOverMenu();
                break;
        }
    }

    void EnterMainMenu()
    {
        CurrentGameState = GameState.MainMenu;

        _BattleUIManager.SwitchUpStatusUIOnOff(false);
        _MenuUIManager.SwitchMainMenuOnOff(true);
        EntireInvCon.Can_TurnOnInv = false;

        Player.transform.position = RespawnPoint.position;
        PlayerBasicAttackCon.Is_Invinsible = true;
        PlayerMovementCon.Can_Control = false;

        BgmCon.BgmPlayMainMenu();
    }

    public void FirstEnterVillage() // enter village from main menu state.
    {
        CurrentGameState = GameState.Prepare;

        _BattleUIManager.SwitchUpStatusUIOnOff(true);
        EntireInvCon.Can_TurnOnInv = true;

        PlayerStatusCon.RefillAllStatusValue();

        Player.transform.position = RespawnPoint.position;
        PlayerBasicAttackCon.Is_Invinsible = true;
        PlayerMovementCon.Can_Control = true;

        BgmCon.BgmPlayPrepare();
    }

    void EnterVillage()
    {
        CurrentGameState = GameState.Prepare;

        EntireInvCon.SwitchInventoryBarrierOnOff(false);
        EntireInvCon.Can_TurnOnInv = true;

        StopCoroutine(_SpellsBattleManager.RestoringMana());
        _SpellsBattleManager.ResetAllTemporaryBuff();
        PlayerSpellsEffectCon.ClearAllSpellsConsistEffect();

        PlayerStatusCon.RefillAllStatusValue();
        HandCardBattleCon.LeaveCombat();

        Player.transform.position = PrepareZoneSpawnPoint.position;
        PlayerBasicAttackCon.Is_Invinsible = true;
        PlayerMovementCon.Can_Control = true;

	    BgmCon.BgmPlayPrepare();
    }

    void EnterCombat()
    {
        CurrentGameState = GameState.Combat;

        EntireInvCon.SwitchInventoryBarrierOnOff(true);
        EntireInvCon.Can_TurnOnInv = true;

        StopCoroutine(_SpellsBattleManager.RestoringMana()); // just in case the coroutine doesn't stop.
        StartCoroutine(_SpellsBattleManager.RestoringMana());
        _SpellsBattleManager.ResetAllTemporaryBuff();
        PlayerSpellsEffectCon.ClearAllSpellsConsistEffect();

        PlayerStatusCon.RefillAllStatusValue();
        HandCardBattleCon.EnterCombatInitialize();

        Player.transform.position = CombatZoneSpawnPoint.position;
        PlayerBasicAttackCon.Is_Invinsible = false;
        PlayerMovementCon.Can_Control = true;

	    BgmCon.BgmPlayCombat();
    }

    void EnterBossFight() // should only enter from combat state.
    {
        CurrentGameState = GameState.BossFight;

        Player.transform.position = BossZoneTeleportPoint.position;

        BgmCon.BgmPlayBossFight();
    }

    void EnterRestartMenu()
    {
        CurrentGameState = GameState.RestartMenu;

        StartCoroutine(_MenuUIManager.StartFadeInRestartMenu());
        EntireInvCon.Can_TurnOnInv = false;

        StopCoroutine(_SpellsBattleManager.RestoringMana());
        _SpellsBattleManager.ResetAllTemporaryBuff();
        PlayerSpellsEffectCon.ClearAllSpellsConsistEffect();

        HandCardBattleCon.LeaveCombat();

        PlayerBasicAttackCon.Is_Invinsible = true;
        PlayerMovementCon.Can_Control = false;

        BgmCon.BgmPlayRestartMenu();
    }


    void EnterGameOverMenu() 
    {
        CurrentGameState = GameState.GameOverMenu;

        StartCoroutine(_MenuUIManager.StartFadeInGameOverMenu());
        EntireInvCon.Can_TurnOnInv = false;

        StopCoroutine(_SpellsBattleManager.RestoringMana());
        _SpellsBattleManager.ResetAllTemporaryBuff();
        PlayerSpellsEffectCon.ClearAllSpellsConsistEffect();

        HandCardBattleCon.LeaveCombat();

        PlayerBasicAttackCon.Is_Invinsible = true;
        PlayerMovementCon.Can_Control = false;

        BgmCon.BgmPlayGameOver();
    }

}
