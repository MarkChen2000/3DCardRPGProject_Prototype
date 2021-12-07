using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleCard_LoaderAndDisplay : MonoBehaviour
{
    public CardData _CardData; // CardData from the ScriptableObject.

    private TMP_Text TMP_CardCost;
    private TMP_Text TMP_CardLV;
    private TMP_Text TMP_CardName;
    private TMP_Text TMP_CardDescription;
    private Sprite Sprite_CardImage;
    private TMP_Text TMP_CardType;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponentofTemplates();
    }
    private void GetComponentofTemplates()
    {
        TMP_CardCost = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        TMP_CardLV = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        Sprite_CardImage = transform.GetChild(0).GetChild(1).GetComponent<Sprite>();
        TMP_CardName = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        TMP_CardDescription = transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<TMP_Text>();
        TMP_CardType = transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<TMP_Text>();
    }

    public void DisplaytoTemplate(CardData carddata) // can get the data from other script and display it.
    {
        _CardData = carddata;
        if (_CardData == null) // if didn't have data, return to default template.
        {
            switch (_CardData._CardType)
            {
                case CardType.Spells:
                    TMP_CardCost.text = "C";
                    break;
                case CardType.Equipment:
                    TMP_CardLV.text = "Lv";
                    break;
            }
            Sprite_CardImage = null;
            TMP_CardName.text = "DefaultName";
            TMP_CardDescription.text = "DefaultDes";
            TMP_CardType.text = "Type";
            return;
        }

        if (_CardData.Card_Image != null) Sprite_CardImage = _CardData.Card_Image;
        TMP_CardName.text = _CardData.CardName;
        TMP_CardDescription.text = _CardData.CardDescription;

        switch (_CardData._CardType)
        {
            case CardType.Spells:
                TMP_CardCost.text = _CardData.CardCost.ToString();
                TMP_CardType.text = "Spells";
                break;
            case CardType.Equipment:
                TMP_CardLV.text = _CardData.CardLv.ToString();
                TMP_CardType.text = "Equipment";
                break;
        }
    }

    public void GetData(CardData carddata) //Here is some conflict ! This function will be replace by DisplayToTemplate().
    {
        _CardData = carddata;
        DisplaytoTemplate(_CardData);
    }
}
