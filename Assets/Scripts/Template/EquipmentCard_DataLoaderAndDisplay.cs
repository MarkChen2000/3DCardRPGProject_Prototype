using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentCard_DataLoaderAndDisplay : MonoBehaviour
{
    public CardData _CardData;
    private Transform Trans_CardPanel;

    private TMP_Text TMP_CardLv;
    private Sprite Sprite_CardImage;
    private TMP_Text TMP_CardName;
    private TMP_Text TMP_CardDescription;

    private void Awake()
    {
        Trans_CardPanel = transform.GetChild(0);
        GetComponentofTemplates();
    }
    
    private void GetComponentofTemplates()
    {
        TMP_CardLv = Trans_CardPanel.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        Sprite_CardImage = Trans_CardPanel.GetChild(1).GetComponent<Sprite>();
        TMP_CardName = Trans_CardPanel.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        TMP_CardDescription = Trans_CardPanel.GetChild(3).GetChild(0).GetComponent<TMP_Text>();
    }

    public void DisplaytoTemplate(CardData carddata)
    {
        _CardData = carddata;
        if (_CardData == null) // if there is no data, 
        {
            //return to default template.
            /*TMP_CardLv.text = "Lv";
            Sprite_CardImage = null;
            TMP_CardName.text = "DefaultName";
            TMP_CardDescription.text = "DefaultDes";*/

            Trans_CardPanel.gameObject.SetActive(false); // don't let the card.
            return;
        }
        else
        {
            if ( Trans_CardPanel.gameObject.activeSelf==false ) Trans_CardPanel.gameObject.SetActive(true);
        }

        TMP_CardLv.text = carddata.CardLv.ToString();
        Sprite_CardImage = carddata.Card_Image;
        TMP_CardName.text = carddata.CardName;
        TMP_CardDescription.text = carddata.CardDescription;

    }

}
