using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChangeGameState Template", menuName = "InteractionTemplate/ChangeGameState")]
public class ChangeGameStateTemplate : ScriptableObject
{
    public string VillagerName = "VillagerA";
    public GameState EnterGameState;

    [TextArea, Tooltip("Saying these text, before get into the ChangeState system")]
    public List<string> BeforeChangeStateSystemTextList = new List<string>();
    [TextArea, Tooltip("Saying these text, before leaving the ChangeState system")]
    public List<string> LeavingChangeStateSystemTextList = new List<string>();
}
