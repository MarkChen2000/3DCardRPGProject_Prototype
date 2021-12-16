using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckCard_DataLoaderAndDisplay : MonoBehaviour
{
    public CardData _CardData;

    private Transform Trans_Panel_DeckCardTemplateBG;
    private TMP_Text TMP_CardName;
    private TMP_Text TMP_CardCost;
    private TMP_Text TMP_CardHoldNum;

    [HideInInspector]
    public int HoldNum = 0; 

    // Start is called before the first frame update
    void Awake()
    {
        GetComponentofTemplates();
    }

    private void GetComponentofTemplates()
    {
        Trans_Panel_DeckCardTemplateBG = transform.GetChild(0);
        TMP_CardCost = Trans_Panel_DeckCardTemplateBG.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        TMP_CardName = Trans_Panel_DeckCardTemplateBG.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        TMP_CardHoldNum = Trans_Panel_DeckCardTemplateBG.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
    }

    public void DisplaytoTemplate(CardData carddata)
    {
        _CardData = carddata;
        if ( _CardData == null )
        {
            TMP_CardCost.text = "C";
            TMP_CardName.text = "DefaultName";
            TMP_CardHoldNum.text = "x0";
            return;
        }

        TMP_CardCost.text = carddata.CardCost.ToString();
        TMP_CardName.text = carddata.CardName;
        TMP_CardHoldNum.text = "x"+ HoldNum.ToString();
    }

}
