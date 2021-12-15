using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation Template", menuName = "InteractionTemplate/Conversation")]
public class ConversationTemplate : ScriptableObject
{
    public string TalkerName = "VillagerA";
    [TextArea]
    public List<string> StringTextList = new List<string>();
}
