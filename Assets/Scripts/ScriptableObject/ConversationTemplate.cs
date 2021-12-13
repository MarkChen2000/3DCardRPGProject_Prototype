using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation Template", menuName = "InteractTemplate/Conversation")]
public class ConversationTemplate : ScriptableObject
{
    public string TalkerName;
    [TextArea]
    public List<string> StringTextList = new List<string>();
}
