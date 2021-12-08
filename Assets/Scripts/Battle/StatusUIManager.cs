using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusUIManager : MonoBehaviour
{
    private TMP_Text TMP_PlayerLV, TMP_PlayerEXP, TMP_PlayerCHP, TMP_PlayerCMana;
    private Slider Slider_PlayerEXP, Slider_PlayerCHP, Slider_PlayerCMana;
    private Transform Trans_UpStatusPanel;

    public PlayerStatus _PlayerStatus;

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
        if (_PlayerStatus == null) _PlayerStatus = Resources.Load<PlayerStatus>("Player/MainPlayer");
        InitializeDisplay();
    }
    
    private void InitializeDisplay()
    {
        DisplayStatus();
    }

    public void DisplayStatus()
    {
        TMP_PlayerLV.text = "LV : " + _PlayerStatus.LV;
        TMP_PlayerEXP.text = _PlayerStatus.nextLVEXP + " / " + _PlayerStatus.EXP;
        TMP_PlayerCHP.text = _PlayerStatus.currentMaxHP + " / " + _PlayerStatus.currentHP;
        TMP_PlayerCMana.text = _PlayerStatus.currentMaxMana + " / " + _PlayerStatus.currentMana;
        Slider_PlayerEXP.maxValue = _PlayerStatus.nextLVEXP;
        Slider_PlayerEXP.value = _PlayerStatus.EXP;
        Slider_PlayerCHP.maxValue = _PlayerStatus.currentMaxHP;
        Slider_PlayerCHP.value = _PlayerStatus.currentHP;
        Slider_PlayerCMana.maxValue = _PlayerStatus.currentMaxMana;
        Slider_PlayerCMana.value = _PlayerStatus.currentMana;
    }

}
