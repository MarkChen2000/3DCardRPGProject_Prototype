using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasicCard_DataLoaderAndDisplay : MonoBehaviour
{
    public CardData _CardData; // CardData from the ScriptableObject.

    // Here are the variable that every basic card will have
    protected TMP_Text TMP_CardCostorLV;
    protected TMP_Text TMP_CardName;
    protected TMP_Text TMP_CardDescription;
    protected Image Image_CardImage;
    protected TMP_Text TMP_CardType;
    protected TMP_Text TMP_CardHoldNum;

    protected Transform Trans_WeaponAPPanel;
    protected Transform Trans_WeaponCRPanel;
    protected Transform Trans_ArmorDPPanel;
    protected Transform Trans_ArmorDSPPane;

    protected TMP_Text TMP_WeaponAP;
    protected TMP_Text TMP_WeaponCR;
    protected TMP_Text TMP_ArmorDP;
    protected TMP_Text TMP_ArmorDSP;

    [HideInInspector] public int HoldNum;

    protected virtual void GetComponentofTemplates() // here will get the need of basic card component,
                                                     // other template's data loader can get their own component by themself.
    {  
        // Here are suppost to be overrided.
    }
    
    public virtual void DisplaytoTemplate(CardData carddata) // can get the data from other script and display it.
    {
        // Here are suppost to be overrided.
    }
}
