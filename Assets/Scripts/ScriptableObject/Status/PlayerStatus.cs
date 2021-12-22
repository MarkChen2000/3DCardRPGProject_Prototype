using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerStatus", menuName = "PlayerStatus")]
public class PlayerStatus : ScriptableObject
{
    // 12.12 note:
    // Because of some mistake, we have to cancle these scriptable object function.
    // By orgin thought, we thought these SO can do the storage function and take/change these variables in it,
    // but now these SO should only have "setting initial value" function for now on,
    // and load in their value to controller at game start,
    // so we move these functions to their own controller.

    public int LV = 1;
    public int EXP = 0;
    public int nextLVEXP = 100; // how much exp till level up.
    public int baseMaxHP = 100;
    //[HideInInspector] public int currentMaxHP; 
    //[HideInInspector] public int currentHP;
    public int basePW = 10;
    //[HideInInspector] public int currentPW; 
    public int baseMP = 10; // Magic Power is difference with Amount of Mana.
    //[HideInInspector] public int currentMP; 
    public int baseMaxMana = 10; // will not increase with lv.
    //[HideInInspector] public int currentMaxMana; 
    //[HideInInspector] public int currentMana;
    public float baseManaRT = 10; 

    public int baseMaxSpeed = 1; 
    //[HideInInspector] public int currentMaxSpeed;

    public int Money = 0;

    private void OnEnable() 
    {
        //InitializeStatus();
    }

    /*private void InitializeStatus()
    {
        currentMaxHP = baseMaxHP;
        currentHP = baseMaxHP;
        currentPW = basePW;
        currentMP = baseMP;
        currentMaxMana = baseMaxMana;
        currentMana = baseMaxMana;
        currentMaxSpeed = baseMaxSpeed;
    }*/

    /*public Dictionary<string, int> GetStatus()
    {
        Dictionary<string, int> statusDict = new Dictionary<string, int>();
        statusDict.Add("LV", this.LV);
        statusDict.Add("EXP", this.EXP);
        statusDict.Add("nextLVEXP", this.nextLVEXP);
        statusDict.Add("baseMaxHP", this.baseMaxHP);
        statusDict.Add("currentMaxHP", this.currentMaxHP);
        statusDict.Add("currentHP", this.currentHP);
        statusDict.Add("basePW", this.basePW);
        statusDict.Add("currentPW", this.currentPW);
        statusDict.Add("baseMP", this.baseMP);
        statusDict.Add("currentMP", this.currentMP);
        statusDict.Add("baseMaxMana", this.baseMaxMana);
        statusDict.Add("currentMaxMana", this.currentMaxMana);
        statusDict.Add("currentMana", this.currentMana);
        statusDict.Add("baseManaRecoveryTime", this.baseManaRT);
        statusDict.Add("baseMaxSpeed", this.baseMaxSpeed);
        statusDict.Add("currentMaxSpeed", this.currentMaxSpeed);
        statusDict.Add("Money", this.Money);

        return statusDict;
    }

    public void UpdateStatus(string name, int value) {
        if (name.Equals("currentMana"))
        {
            this.currentMana += value;
        }
    }*/
}
