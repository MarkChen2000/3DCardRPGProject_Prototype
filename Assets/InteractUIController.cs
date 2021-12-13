using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractUIController : MonoBehaviour
{
    private InteractController InteractCon;

    private GameObject Panel_InteractInfoBG;
    private TMP_Text TMP_InteractInfo;
    private GameObject Panel_ConversationUIBG;
    private TMP_Text TMP_TalkerName;
    private TMP_Text TMP_ConversationText;
    private GameObject Panel_ShopUIBG;
    private GameObject Panel_MissionUIBG;

    private void Awake()
    {
        InteractCon = GetComponent<InteractController>();

        Panel_InteractInfoBG = transform.GetChild(0).GetChild(0).gameObject;
        Panel_ConversationUIBG = transform.GetChild(0).GetChild(1).gameObject;
        TMP_TalkerName = Panel_ConversationUIBG.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        TMP_ConversationText = Panel_ConversationUIBG.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        Panel_ShopUIBG = transform.GetChild(0).GetChild(2).gameObject;
        Panel_MissionUIBG = transform.GetChild(0).GetChild(3).gameObject;

        TMP_InteractInfo = Panel_InteractInfoBG.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    void Start()
    {
        HideAllInteractUI();
        HideInteractInfo();
    }

    public void HideAllInteractUI()
    {
        Panel_ConversationUIBG.SetActive(false);
        Panel_MissionUIBG.SetActive(false);
        Panel_ShopUIBG.SetActive(false);
    }

    public void ShowInteractInfo(InteractType type)
    {
        Panel_InteractInfoBG.SetActive(true);
        TMP_InteractInfo.text = type.ToString() + " ( E ) ";
    }

    public void HideInteractInfo()
    {
        Panel_InteractInfoBG.SetActive(false);
    }

    private int textlistcount;
    private ConversationTemplate con_template;

    public void StartDisplayConversaionText(ConversationTemplate template) // Do conversation function.
    {
        con_template = template;
        textlistcount = 0;
        Panel_ConversationUIBG.SetActive(true);
        TMP_TalkerName.text = con_template.TalkerName;
        TMP_ConversationText.text = con_template.StringTextList[textlistcount];
    }

    public bool DisplayNextConversationText()
    {
        textlistcount++;
        if ( textlistcount < con_template.StringTextList.Count )
        {
            TMP_ConversationText.text = con_template.StringTextList[textlistcount];
            return true;
        }
        else return false;
    }

}
