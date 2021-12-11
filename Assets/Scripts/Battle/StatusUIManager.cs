using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum StatusType
{
    LVEXP,HP,Mana
}


public class StatusUIManager : MonoBehaviour
{
    private TMP_Text TMP_PlayerLV, TMP_PlayerEXP, TMP_PlayerCHP, TMP_PlayerCMana;
    private Slider Slider_PlayerEXP, Slider_PlayerCHP, Slider_PlayerCMana;
    private Transform Trans_UpStatusPanel;

    private PlayerStatusController PlayerStatusCon;

    private void Awake()
    {
        Trans_UpStatusPanel = transform.GetChild(0).GetChild(0);
        TMP_PlayerLV = Trans_UpStatusPanel.GetChild(0).GetComponent<TMP_Text>();
        Slider_PlayerEXP = Trans_UpStatusPanel.GetChild(1).GetComponent<Slider>();
        TMP_PlayerEXP = Trans_UpStatusPanel.GetChild(2).GetComponent<TMP_Text>();
        Slider_PlayerCHP = Trans_UpStatusPanel.GetChild(4).GetComponent<Slider>();
        TMP_PlayerCHP = Trans_UpStatusPanel.GetChild(5).GetComponent<TMP_Text>();
        Slider_PlayerCMana = Trans_UpStatusPanel.GetChild(7).GetComponent<Slider>();
        TMP_PlayerCMana = Trans_UpStatusPanel.GetChild(8).GetComponent<TMP_Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerStatusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
        InitializeDisplay();
    }
    
    private void InitializeDisplay()
    {
        UpdateAllStatusDisplay();
    }

    public void UpdateAllStatusDisplay()
    {
        TMP_PlayerLV.text = "LV : " + PlayerStatusCon.LV;
        TMP_PlayerEXP.text = PlayerStatusCon.nextLVEXP + " / " + PlayerStatusCon.EXP;
        TMP_PlayerCHP.text = PlayerStatusCon.currentMaxHP + " / " + PlayerStatusCon.currentHP;
        TMP_PlayerCMana.text = PlayerStatusCon.currentMaxMana + " / " + PlayerStatusCon.currentMana;
        Slider_PlayerEXP.maxValue = PlayerStatusCon.nextLVEXP;
        Slider_PlayerEXP.value = PlayerStatusCon.EXP;
        Slider_PlayerCHP.maxValue = PlayerStatusCon.currentMaxHP;
        Slider_PlayerCHP.value = PlayerStatusCon.currentHP;
        Slider_PlayerCMana.maxValue = PlayerStatusCon.currentMaxMana;
        Slider_PlayerCMana.value = PlayerStatusCon.currentMana;
    }

    public void UpdateOneStatusDisplay(StatusType type)
    {
        switch ( type )
        {
            case StatusType.LVEXP:
                TMP_PlayerLV.text = "LV : " + PlayerStatusCon.LV;
                TMP_PlayerEXP.text = PlayerStatusCon.nextLVEXP + " / " + PlayerStatusCon.EXP;
                Slider_PlayerEXP.maxValue = PlayerStatusCon.nextLVEXP;
                Slider_PlayerEXP.value = PlayerStatusCon.EXP;
                break;
            case StatusType.HP:
                TMP_PlayerCHP.text = PlayerStatusCon.currentMaxHP + " / " + PlayerStatusCon.currentHP;
                Slider_PlayerCHP.maxValue = PlayerStatusCon.currentMaxHP;
                Slider_PlayerCHP.value = PlayerStatusCon.currentHP;
                break;
            case StatusType.Mana:
                TMP_PlayerCMana.text = PlayerStatusCon.currentMaxMana + " / " + PlayerStatusCon.currentMana;
                Slider_PlayerCMana.maxValue = PlayerStatusCon.currentMaxMana;
                Slider_PlayerCMana.value = PlayerStatusCon.currentMana;
                break;
        }
    }

}
