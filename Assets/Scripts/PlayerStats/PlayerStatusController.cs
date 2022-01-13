using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatusController : MonoBehaviour
{
    [Tooltip("If you leave this reference as null, it will automatical reference the gameplay data.\nIn other word, you can reference testing data if you want to, but eventually need to leave this as null before building.")]
    public PlayerStatus _PlayerStatus;

    // "base/baseMax" mean the character basic value
    // "currentMax" mean the value was plused by equipment bonus.
    // "current" mean the the current remaining amount of value.
    [HideInInspector] public int LV = 1;
    [HideInInspector] public int EXP = 0;
    [HideInInspector] public int nextLVEXP = 100; // how much exp till level up.
    [HideInInspector] public int baseMaxHP = 100;
    [HideInInspector] public int currentMaxHP; /*{ get ; private set; }*/
    [HideInInspector] public int currentHP; /*{ get ; private set; }*/
    [HideInInspector] public int basePW = 10;
    [HideInInspector] public int currentPW;
    [HideInInspector] public int baseMP = 10; // Magic Power is difference with Amount of Mana.
    [HideInInspector] public int currentMP;
    [HideInInspector] public int baseMaxMana = 10; // will not increase with lv.
    [HideInInspector] public int currentMaxMana;
    [HideInInspector] public int currentMana;
    [HideInInspector] public float baseManaRT = 10;
    //public float baseManaRT = 2;
    [HideInInspector] public float currentManaRT;
    [HideInInspector] public float baseSP = 100; 
    [HideInInspector] public float currentSP;
    [HideInInspector] public int Money = 0;

    private StatusUIManager _StatusUIManager;


    // Start is called before the first frame update
    void Awake()
    {
        SaveandLoadPlayerStatus(true); // Load
        if (_PlayerStatus == null) _PlayerStatus = Resources.Load<PlayerStatus>("Player/MainPlayer");
        // Load initial player status asset first, this may be replaced by Save and Load System before build.
        InitializeLoadinData();

        _StatusUIManager = GameObject.Find("BattleUI").GetComponent<StatusUIManager>();
    }

    public void SaveandLoadPlayerStatus(bool SorL)
    {
        // save and load system
        if (SorL)
        {
        }
        else
        {
        }
    }

    void InitializeLoadinData()
    {
        LV = _PlayerStatus.LV;
        EXP = _PlayerStatus.EXP;
        nextLVEXP = _PlayerStatus.nextLVEXP;
        baseMaxHP = _PlayerStatus.baseMaxHP;
        basePW = _PlayerStatus.basePW;
        baseMP = _PlayerStatus.baseMP;
        baseMaxMana = _PlayerStatus.baseMaxMana;
        baseManaRT = _PlayerStatus.baseManaRT;
        baseSP = _PlayerStatus.baseSP;
        Money = _PlayerStatus.Money;

        currentMaxHP = baseMaxHP;
        currentHP = baseMaxHP;
        currentPW = basePW;
        currentMP = baseMP;
        currentMaxMana = baseMaxMana;
        currentMana = baseMaxMana;
        currentManaRT = baseManaRT;
        currentSP = baseSP;
    }

    // If you want to secure the safty of variables, can use { get; private set } method to protect variables from changing by other scripts.
    // But I think that is not necessary in this situation.

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

        *//*statusDict.Add("baseManaRecoveryTime", this.baseManaRT); // These variables have to be float type !! So can not fit into here...
        statusDict.Add("baseSP", this.baseSP);
        statusDict.Add("currentSP", this.currentSP);*//*

        statusDict.Add("Money", this.Money);

        return statusDict;
    }*/

    /*public void UpdateStatus(string name, int value)
    {
        if (name.Equals("currentMana"))
        {
            this.currentMana += value;
            this._StatusUIManager.UpdateAllStatusDisplay();
        }
        else if (name.Equals("currentHP"))
        {
            if (value < 0)
            {
                this.currentHP += value;
            }
            else if (value > 0)
            {
                if (this.currentHP < this.currentMaxHP - value)
                {
                    this.currentHP += value;
                }
                else if (this.currentHP > this.currentHP - value && this.currentHP != this.currentMaxHP)
                {
                    this.currentHP = this.currentMaxHP;
                }
            }

            this._StatusUIManager.UpdateAllStatusDisplay();
        }
    }*/

    public void SpellsCostMana(int cost)
    {
        Debug.Log("Cost " + cost + " mana(s)!");
        currentMana = (int)Mathf.Clamp(currentMana - cost, 0, currentMaxMana);
        _StatusUIManager.UpdateOneStatusDisplay(StatusType.Mana);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        _StatusUIManager.UpdateOneStatusDisplay(StatusType.HP);

        if ( currentHP <= 0 )
        {
            PlayerDead();
        }
    }

    public void RefillAllStatusValue()
    {
        currentHP = currentMaxHP;
        currentMana = currentMaxMana;
        _StatusUIManager.UpdateAllStatusDisplay();
    }

    public void Spells_RestoreValue(SpellsRestoreType type,int restore_amount)
    {
        switch ( type )
        {
            case SpellsRestoreType.HP:
                RestoreHP(restore_amount);
                break;
            case SpellsRestoreType.Mana:
                RestoreMana(restore_amount);
                break;
        }
    }

    public IEnumerator RestoringMana() 
    {
        while ( true ) // every frame check the mana is leas then max value.
        {
            while ( currentMana < currentMaxMana )
            {
                yield return new WaitForSeconds(currentManaRT); // after wait for RT then restore 1 mana.
                RestoreMana(1);
            }
            yield return null;
        }
    }

    public void Spells_TemporaryBuff(SpellsBuffType type, float buffvalue, int duration)
    {
        int disparity = 0;
        switch (type)
        {
            case SpellsBuffType.PW:
                disparity = Mathf.RoundToInt(currentPW * buffvalue);
                currentPW += disparity;
                break;
            case SpellsBuffType.MP:
                disparity = Mathf.RoundToInt(currentMP * buffvalue);
                currentMP += disparity;
                break;
            case SpellsBuffType.RT:
                disparity = (int)buffvalue;
                currentManaRT -= disparity;
                break;
            case SpellsBuffType.SP:
                disparity = (int)buffvalue;
                currentSP += disparity;
                break;
        }
        Debug.Log("Buff " + type.ToString() + " by " + disparity);
        StartCoroutine(WaitforBuffEnd(type, duration, disparity));
    }

    IEnumerator WaitforBuffEnd(SpellsBuffType type, int duraion, int disparity)
    {
        yield return new WaitForSeconds(duraion);
        switch (type)
        {
            case SpellsBuffType.PW:
                currentPW -= disparity;
                break;
            case SpellsBuffType.MP:
                currentMP -= disparity;
                break;
            case SpellsBuffType.RT:
                currentManaRT += disparity;
                break;
            case SpellsBuffType.SP:
                currentSP -= disparity;
                break;
        }
        Debug.Log("Return buff " + type.ToString() + " by " + disparity);
    }

    void RestoreMana(int restoreamount)
    {
        Debug.Log("Restore Mana:"+ restoreamount);
        currentMana = (int)Mathf.Clamp(currentMana + restoreamount, 0, currentMaxMana);
        _StatusUIManager.UpdateOneStatusDisplay(StatusType.Mana);
    }

    void RestoreHP(int restoreamount)
    {
        Debug.Log("Restore HP:" + restoreamount);
        //Debug.Log("CurrentHP:" + currentHP + " CurrentMaxHP:" + currentMaxHP + " RestoreAmount:" + restoreamount);
        currentHP = (int)Mathf.Clamp(currentHP + restoreamount, 0, currentMaxHP);
        _StatusUIManager.UpdateOneStatusDisplay(StatusType.HP);
    }

    public void GainMoney(int gain)
    {
        Debug.Log("Player gains " + gain + " money!");
        Money += gain;
        _StatusUIManager.UpdateOneStatusDisplay(StatusType.Money);
    }

    void PlayerDead() // Reload the scene.
    {
        SceneManager.LoadScene("Scene_Field");
    }

}
