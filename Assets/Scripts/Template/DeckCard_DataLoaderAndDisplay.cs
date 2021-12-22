using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckCard_DataLoaderAndDisplay : BasicCard_DataLoaderAndDisplay
{
    private Transform Trans_CardPanel;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponentofTemplates();
    }

    protected override void GetComponentofTemplates()
    {
        Trans_CardPanel = transform.GetChild(0);
        Image_CardImage = Trans_CardPanel.GetChild(0).GetChild(0).GetComponent<Image>();
        TMP_CardCostorLV = Trans_CardPanel.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        TMP_CardName = Trans_CardPanel.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        TMP_CardHoldNum = Trans_CardPanel.GetChild(3).GetChild(0).GetComponent<TMP_Text>();
    }

    public override void DisplaytoTemplate(CardData carddata)
    {
        _CardData = carddata;
        if ( _CardData == null )
        {
            TMP_CardCostorLV.text = "C";
            TMP_CardName.text = "DefaultName";
            TMP_CardHoldNum.text = "x0";
            return;
        }
        Image_CardImage.sprite = _CardData.Card_ImageSprite;
        TMP_CardCostorLV.text = _CardData.CardCost.ToString();
        TMP_CardName.text = _CardData.CardName;
        TMP_CardHoldNum.text = "x"+ HoldNum.ToString();
    }

}
