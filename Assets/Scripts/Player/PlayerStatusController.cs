using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatusController : MonoBehaviour
{

    public PlayerStatus _PlayerStatus;

    // "base/baseMax" mean the character basic value
    // "currentMax" mean the value was plused by equipment bonus.
    // "current" mean the the current remaining amount of value.
    public int LV = 1;
    [HideInInspector] public int EXP = 0;
    public int nextLVEXP = 100; // how much exp till level up.
    public int baseMaxHP = 100;
    [HideInInspector] public int currentMaxHP;
    [HideInInspector] public int currentHP;
    public int basePW = 10;
    [HideInInspector] public int currentPW;
    public int baseMP = 10; // Magic Power is difference with Amount of Mana.
    [HideInInspector] public int currentMP;
    public int baseMaxMana = 10; // will not increase with lv.
    [HideInInspector] public int currentMaxMana;
    [HideInInspector] public int currentMana;
    public float baseManaRT = 10;
    public float currentMaxManaRT;

    public float baseMaxSpeed = 1; // will not increase with lv.
    [HideInInspector] public float currentMaxSpeed;

    private StatusUIManager statusUIManager;

    public int Money = 0;

    // Start is called before the first frame update
    void Awake()
    {
        SaveandLoadPlayerStatus(true); // Load
        if (_PlayerStatus == null) _PlayerStatus = Resources.Load<PlayerStatus>("Player/MainPlayer");
        // Load initial player status asset first, this may be replaced by Save and Load System before build.
        InitializeLoadinData();

        this.statusUIManager = GameObject.Find("BattleUI").GetComponent<StatusUIManager>();
    }

    void Start()
    {
        InvokeRepeating("AutoRestoreMP", 0f, this.baseManaRT);
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

    void Update()
    {
        if (this.currentHP <= 0)
        {
            SceneManager.LoadScene("Scene_Field");
        }
    }


    private void InitializeLoadinData()
    {
        LV = _PlayerStatus.LV;
        EXP = _PlayerStatus.EXP;
        nextLVEXP = _PlayerStatus.nextLVEXP;
        baseMaxHP = _PlayerStatus.baseMaxHP;
        basePW = _PlayerStatus.basePW;
        baseMP = _PlayerStatus.baseMP;
        baseMaxMana = _PlayerStatus.baseMaxMana;
        baseManaRT = _PlayerStatus.baseManaRT;
        baseMaxSpeed = _PlayerStatus.baseMaxSpeed;
        Money = _PlayerStatus.Money;

        currentMaxHP = baseMaxHP;
        currentHP = baseMaxHP;
        currentPW = basePW;
        currentMP = baseMP;
        currentMaxMana = baseMaxMana;
        currentMana = baseMaxMana;
        currentMaxManaRT = baseManaRT;
        currentMaxSpeed = baseMaxSpeed;
    }

    public Dictionary<string, int> GetStatus()
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

        /*statusDict.Add("baseManaRecoveryTime", this.baseManaRT); // These variables have to be float type !! So can not fit into here...
        statusDict.Add("baseMaxSpeed", this.baseMaxSpeed);
        statusDict.Add("currentMaxSpeed", this.currentMaxSpeed);*/

        statusDict.Add("Money", this.Money);

        return statusDict;
    }

    public void UpdateStatus(string name, int value)
    {
        if (name.Equals("currentMana"))
        {
            this.currentMana += value;
            this.statusUIManager.UpdateAllStatusDisplay();
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

            this.statusUIManager.UpdateAllStatusDisplay();
        }
    }

    private void AutoRestoreMP()
    {
        if (this.currentMana < this.currentMaxMana)
        {
            this.currentMana++;
            this.statusUIManager.UpdateAllStatusDisplay();
        }
    }

    public void GainMoney(int gain)
    {
        Money += gain;
        statusUIManager.UpdateOneStatusDisplay(StatusType.Money);
    }
}
