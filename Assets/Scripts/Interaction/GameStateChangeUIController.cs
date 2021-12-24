using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStateChangeUIController : MonoBehaviour
{
    private TMP_Text Button_Text;
    private GameState WaitforEnterState;
    private GameStateController GameStateCon;
    InteractionController InteractionCon;

    // Start is called before the first frame update
    void Awake()
    {
        GameStateCon = GameObject.Find("GameManager").GetComponent<GameStateController>();
        InteractionCon = GameObject.Find("InteractAndUIManager").GetComponent<InteractionController>();
        Button_Text = transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
    }

    public void EnterChangeStateSystem(ChangeGameStateTemplate changestate_temp)
    {
        Button_Text.text = changestate_temp.EnterGameState.ToString();
        WaitforEnterState = changestate_temp.EnterGameState;
    }

    public void ChangeStateButtonPress()
    {
        GameStateCon.EnterState(WaitforEnterState);
        InteractionCon.PlayerEndInteraction();
    }

}
