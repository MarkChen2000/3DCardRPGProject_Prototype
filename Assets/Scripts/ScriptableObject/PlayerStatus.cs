using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerStatus", menuName = "PlayerStatus")]
public class PlayerStatus : ScriptableObject
{

    public int LV = 1;
    public int baseMaxHP = 100;
    private int currentHP;
    public int basePW = 10;
    public int baseMP = 10; // Magic Power is difference with Amount of Mana.
    public int baseMaxMana = 10;
    private int currentMana;
    public int baseManaRT = 10; // it mean how much time will regain mana.
    private int weaponAP;
    private int weaponCR; //Weapon critical rate.
    private int armorDP;

    public Dictionary<string, int> GetStatus()
    {
        Dictionary<string, int> statusDict = new Dictionary<string, int>();
        statusDict.Add("LV", this.LV);
        statusDict.Add("baseMaxHP", this.baseMaxHP);
        statusDict.Add("currentHP", this.currentHP);
        statusDict.Add("basePW", this.basePW);
        statusDict.Add("baseMP", this.baseMP);
        statusDict.Add("baseMaxMana", this.baseMaxMana);
        statusDict.Add("currentMana", this.currentMana);
        statusDict.Add("baseManaRecoveryTime", this.baseManaRT);
        statusDict.Add("WeaponAP", this.weaponAP);
        statusDict.Add("WeaponCR", this.weaponCR);
        statusDict.Add("ArmorDP", this.armorDP);

        return statusDict;
    }

    public void UpdateStatus(string name, int value) {
        if (name.Equals("currentMana"))
        {
            this.currentMana += value;
        }
    }
}
