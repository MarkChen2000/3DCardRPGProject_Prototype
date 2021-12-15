using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionUIController : MonoBehaviour
{
    // This Interaction UI controller only control turn on/off the UI system's panel,
    // The control of detailed UI in every system is control by each controller of interaction system, such as shop system... , 
    // ecept for conversation control.

    private InteractionController InteractCon;

    private GameObject Panel_InteractionInfoBG;
    private TMP_Text TMP_InteractionInfo;
    private GameObject Panel_ConversationUIBG;
    private TMP_Text TMP_TalkerName;
    private TMP_Text TMP_ConversationText;
    private GameObject Panel_ShopUIBG;
    private GameObject Panel_MissionUIBG;

    private void Awake()
    {
        InteractCon = GetComponent<InteractionController>();

        Panel_InteractionInfoBG = transform.GetChild(0).GetChild(0).gameObject;
        Panel_ConversationUIBG = transform.GetChild(0).GetChild(1).gameObject;
        TMP_TalkerName = Panel_ConversationUIBG.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        TMP_ConversationText = Panel_ConversationUIBG.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        Panel_ShopUIBG = transform.GetChild(0).GetChild(2).gameObject;
        Panel_MissionUIBG = transform.GetChild(0).GetChild(3).gameObject;

        TMP_InteractionInfo = Panel_InteractionInfoBG.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    void Start()
    {
        HideAllInteractionUI();
        HideInteractionInfo();
    }

    public void HideAllInteractionUI()
    {
        Panel_ConversationUIBG.SetActive(false);
        Panel_ShopUIBG.SetActive(false);
        Panel_MissionUIBG.SetActive(false);
    }

    public void ShowInteractionInfo(InteractionType type)
    {
        Panel_InteractionInfoBG.SetActive(true);
        TMP_InteractionInfo.text = type.ToString() + " ( E ) ";
    }

    public void HideInteractionInfo()
    {
        Panel_InteractionInfoBG.SetActive(false);
    }

    private int textlistcount;
    private List<string> conversationTextList ;

    public void StartDisplayConversaionText(string talkername, List<string> textlist) // Do conversation function.
    {
        conversationTextList = textlist;
        textlistcount = 0;

        Panel_ConversationUIBG.SetActive(true);
        TMP_TalkerName.text = talkername;
        TMP_ConversationText.text = conversationTextList[textlistcount];
    }

    public bool DisplayNextConversationText()
    {
        textlistcount++;
        if ( textlistcount < conversationTextList.Count )
        {
            TMP_ConversationText.text = conversationTextList[textlistcount];
            return true;
        }
        else return false;
    }

    public void ShowShopUI()
    {
        Panel_ShopUIBG.SetActive(true);
    }

}
