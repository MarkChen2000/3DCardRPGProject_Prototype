using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    SubmitCard, LevelUp, KillMonster
}

[CreateAssetMenu(fileName ="New Mission Template",menuName = "InteractionTemplate/Mission")]
public class MissionTemplate : ScriptableObject
{
    public string MissionName = "Testing MissionA" ;
    public MissionType _MissionType;

    [Header("For SubmitItem Mission")]
    public CardData SubmitCard ;
    public int AmountofSubmit = 1;

    [Header("For LevelUp Mission")]
    public int ReachLv = 1;

    [Header("For KillMonster Mission")]
    public string MonsterName;
    public int AmountofMonster = 1;

    [TextArea, Tooltip("Saying these text, before get into the MissionUI")]
    public List<string> BeforeInteractionTextList = new List<string>();

    [TextArea, Tooltip("Saying these text, before leaving the MissionUI")]
    public List<string> LeavingInteractionTextList = new List<string>();
}
