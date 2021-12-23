using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusLevelupSystem : MonoBehaviour
{
    private PlayerStatusController PlayerStatusCon;
    private StatusUIManager _StatusUIManager;
    private EquipmentSlotController EquipmentClotCon;

    public int IncreasenextLVEXPperLV = 100;
    public int IncreaseMaxHPperLV = 100; 
    public int IncreaseMPperLV = 10;
    public int IncreasePWperLV = 10;
    public int IncreaseMaxManaperLV = 0;
    public int IncreaseSPperLV = 5;
    public float DecreaseManaRTperLV = 0.9f;

    private void Awake()
    {
        _StatusUIManager = GameObject.Find("BattleUI").GetComponent<StatusUIManager>();
        EquipmentClotCon = GameObject.Find("InventoryAndUIManager").GetComponent<EquipmentSlotController>();
        PlayerStatusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
    }

    private void PlayerStatusIncreaseOneLV()
    {
        PlayerStatusCon.nextLVEXP += IncreasenextLVEXPperLV;
        PlayerStatusCon.baseMaxHP += IncreaseMaxHPperLV;
        PlayerStatusCon.baseMP += IncreaseMPperLV;
        PlayerStatusCon.basePW += IncreasePWperLV;
        PlayerStatusCon.baseMaxMana += IncreaseMaxManaperLV;
        PlayerStatusCon.baseSP += IncreaseSPperLV;
        PlayerStatusCon.baseManaRT *= DecreaseManaRTperLV;
    }

    public void GainExp(int gain)
    {
        Debug.Log("Player gain " + gain + "exp!");
        PlayerStatusCon.EXP += gain;
        _StatusUIManager.UpdateOneStatusDisplay(StatusType.LVEXP);
        while ( PlayerStatusCon.EXP >= PlayerStatusCon.nextLVEXP )
        {
            PlayerStatusCon.EXP -= PlayerStatusCon.nextLVEXP;
            LevelUp();
        }
    }

    private void LevelUp() // Level growing up by only 1.
    {
        PlayerStatusCon.LV++;
        PlayerStatusIncreaseOneLV();
        EquipmentClotCon.AddEquipBonusValuetoPlayerStatus();
        _StatusUIManager.UpdateAllStatusDisplay();
    }

}
