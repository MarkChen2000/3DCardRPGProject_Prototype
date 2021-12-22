using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentCard_DataLoaderAndDisplay : BasicCard_DataLoaderAndDisplay
{
    private Transform Trans_CardPanel;

    private void Awake()
    {
        GetComponentofTemplates();
    }

    protected override void GetComponentofTemplates()
    {
        Trans_CardPanel = transform.GetChild(0);
        Image_CardImage = Trans_CardPanel.GetChild(0).GetChild(0).GetComponent<Image>();
        TMP_CardCostorLV = Trans_CardPanel.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        TMP_CardName = Trans_CardPanel.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        TMP_CardDescription = Trans_CardPanel.GetChild(3).GetChild(0).GetComponent<TMP_Text>();
        TMP_CardType = Trans_CardPanel.GetChild(4).GetChild(0).GetComponent<TMP_Text>();

        Trans_WeaponAPPanel = Trans_CardPanel.GetChild(5);
        Trans_WeaponCRPanel = Trans_CardPanel.GetChild(6);
        Trans_ArmorDPPanel = Trans_CardPanel.GetChild(7);
        Trans_ArmorDSPPane = Trans_CardPanel.GetChild(8);

        TMP_WeaponAP = Trans_CardPanel.GetChild(5).GetChild(0).GetComponent<TMP_Text>();
        TMP_WeaponCR = Trans_CardPanel.GetChild(6).GetChild(0).GetComponent<TMP_Text>();
        TMP_ArmorDP = Trans_CardPanel.GetChild(7).GetChild(0).GetComponent<TMP_Text>();
        TMP_ArmorDSP = Trans_CardPanel.GetChild(8).GetChild(0).GetComponent<TMP_Text>();
    }

    public override void DisplaytoTemplate(CardData carddata)
    {
        _CardData = carddata;
        if (_CardData == null) // if there is no data, 
        {
            Trans_CardPanel.gameObject.SetActive(false); // don't let the card display.
            return;
        }
        else
        {
            if ( Trans_CardPanel.gameObject.activeSelf==false ) Trans_CardPanel.gameObject.SetActive(true);
        }

        TMP_CardCostorLV.text = carddata.CardLv.ToString();
        Image_CardImage.sprite = _CardData.Card_ImageSprite;
        TMP_CardName.text = carddata.CardName;
        TMP_CardDescription.text = carddata.CardDescription;
        TMP_CardType.text = carddata._EquipmentType.ToString();

        switch (carddata._EquipmentType)
        {
            case EquipmentType.Weapon:
                Trans_WeaponAPPanel.gameObject.SetActive(true);
                Trans_WeaponCRPanel.gameObject.SetActive(true);
                Trans_ArmorDPPanel.gameObject.SetActive(false);
                Trans_ArmorDSPPane.gameObject.SetActive(false);
                TMP_WeaponAP.text = carddata.WeaponAP.ToString();
                TMP_WeaponCR.text = carddata.WeaponCR.ToString();
                break;
            case EquipmentType.Armor:
                Trans_WeaponAPPanel.gameObject.SetActive(false);
                Trans_WeaponCRPanel.gameObject.SetActive(false);
                Trans_ArmorDPPanel.gameObject.SetActive(true);
                Trans_ArmorDSPPane.gameObject.SetActive(true);
                TMP_ArmorDP.text = carddata.ArmorDP.ToString();
                TMP_ArmorDSP.text = carddata.ArmorDSP.ToString();
                break;
            case EquipmentType.Ornament:
                Trans_WeaponAPPanel.gameObject.SetActive(false);
                Trans_WeaponCRPanel.gameObject.SetActive(false);
                Trans_ArmorDPPanel.gameObject.SetActive(false);
                Trans_ArmorDSPPane.gameObject.SetActive(false);
                break;
            case EquipmentType.NotEquip:
                Debug.Log("This is not Equipment!");
                Trans_WeaponAPPanel.gameObject.SetActive(false);
                Trans_WeaponCRPanel.gameObject.SetActive(false);
                Trans_ArmorDPPanel.gameObject.SetActive(false);
                Trans_ArmorDSPPane.gameObject.SetActive(false);
                break;
        }
    }

}
