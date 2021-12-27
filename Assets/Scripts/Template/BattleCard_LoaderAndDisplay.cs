using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleCard_LoaderAndDisplay : BasicCard_DataLoaderAndDisplay
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponentofTemplates();
    }

    protected override void GetComponentofTemplates()
    {
        Image_CardImage = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        TMP_CardCostorLV = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        TMP_CardName = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        TMP_CardDescription = transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<TMP_Text>();
        TMP_CardType = transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<TMP_Text>();
    }

    public override void DisplaytoTemplate(CardData carddata) // can get the data from other script and display it.
    {
        _CardData = carddata;
        if (_CardData == null) // if didn't have data, return to default template.
        {
            switch (_CardData._CardType)
            {
                case CardType.Spells:
                    TMP_CardCostorLV.text = "C";
                    break;
                case CardType.Equipment:
                    //Debug.Log("This is not Spells! Battle Card should only contain spells!");
                    break;
            }

            TMP_CardName.text = "DefaultName";
            TMP_CardDescription.text = "DefaultDes";
            TMP_CardType.text = "Type";
            return;
        }

        if (_CardData.Card_ImageSprite == null)
        {
        }
        else
        {
            Image_CardImage.sprite = _CardData.Card_ImageSprite;
        }
        //Debug.Log(Image_CardImage.sprite);
        TMP_CardName.text = _CardData.CardName;
        TMP_CardDescription.text = _CardData.CardDescription;

        switch (_CardData._CardType)
        {
            case CardType.Spells:
                TMP_CardCostorLV.text = _CardData.CardCost.ToString();
                TMP_CardType.text = "Spells";
                break;
            case CardType.Equipment:
                Debug.Log("This is not Spells! Battle Card should only contain spells!");
                break;
        }
    }
}
