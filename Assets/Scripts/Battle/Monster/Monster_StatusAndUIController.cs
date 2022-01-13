using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Monster_StatusAndUIController : MonoBehaviour
{
    public bool Use_UIPrefabSystem = false;
    public GameObject HealthUI_Prefab;
    Transform Trans_DisplayUICanvas;
    Transform UIDisplaySpot;
    Camera MainCamera;

    public GameObject DeathEffectPrefab;

    //TextMeshProUGUI _text;
    private BattleUIManager _BattleUIManager;
    private BattleValueCalculator BattleValueCal;
    private PlayerStatusLevelupSystem PlayerLevelupSystem;
    private PlayerStatusController PlayerStatusCon;
    private Monster_BehaviorController Monster_BehaviorCon;

    public MonsterStatus _MonsterStatus;
    private int currentHP;


    // Start is called before the first frame update
    void Start()
    {
        if (Use_UIPrefabSystem)
        {
            // Trans_DisplayUICanvas = transform.GetChild(1);
            Trans_DisplayUICanvas = GameObject.Find("BattleUI").transform.GetChild(1).GetChild(0);
            UIDisplaySpot = transform.GetChild(2);
            MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        }

        //_text = this.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

        currentHP = _MonsterStatus.max_hp;
        this.updateStatus(0);

        _BattleUIManager = GameObject.Find("BattleUI").GetComponent<BattleUIManager>();
        BattleValueCal = GameObject.Find("BattleManager").GetComponent<BattleValueCalculator>();
        PlayerLevelupSystem = GameObject.Find("PlayerManager").GetComponent<PlayerStatusLevelupSystem>();
        PlayerStatusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
        Monster_BehaviorCon = GetComponent<Monster_BehaviorController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Use_UIPrefabSystem)
        {
            CheckShoworHideUIDisplay();
        }
        Monster_BehaviorCon.current_hp_percentage = (int)((currentHP / _MonsterStatus.max_hp) * 100);
    }

    private void FixedUpdate()
    {
        if (Use_UIPrefabSystem)
        {
            UpdateUIPos();
        }
    }

    public void beAttacked(Vector2 damageinfo)
    {
        int damage = (int)damageinfo.y;
        damage = BattleValueCal.EnemyTakeDamageCalculate(damage);

        _BattleUIManager.SpawnPopupDamageUI(damageinfo, UIDisplaySpot);

        updateStatus(damage);

        if (Use_UIPrefabSystem)
        {
            UpdateUIStatus();
        }

        if (currentHP <= 0)
        {
            MonsterDead();
        }

        //Debug.Log(damage);
    }

    public void updateStatus(int number)
    {
        //_text.SetText("HP :" + (50 - number));
        //_text.text = "HP :" + (currentHP - number);
        currentHP = currentHP - number;
    }

    private void MonsterDead()
    {
        PlayerLevelupSystem.GainExp(_MonsterStatus.Drop_Exp);
        PlayerStatusCon.GainMoney(_MonsterStatus.DropMoney);

        if (DeathEffectPrefab != null)
            Instantiate(DeathEffectPrefab, transform.position, Quaternion.identity);

        DestroyDisplayUI();
        Destroy(this.gameObject);
    }

    private void CheckShoworHideUIDisplay()
    {
        if (Monster_BehaviorCon.OnAttackMode)
        {
            if (!Using_UIPrefab)
            {
                SpawnDisplayUI();
            }
        }
        else
        {
            if (Using_UIPrefab)
            {
                DestroyDisplayUI();
            }
        }
    }

    GameObject CurrentPrefab;
    Slider CurrentSlider;
    TMP_Text CurrentHPText;
    TMP_Text CurrentLVandName;
    bool Using_UIPrefab = false; // True mean that currently have prefab was created.

    public void SpawnDisplayUI()
    {
        CurrentPrefab = Instantiate(HealthUI_Prefab, Trans_DisplayUICanvas);
        CurrentSlider = CurrentPrefab.transform.GetChild(0).GetComponent<Slider>();
        CurrentHPText = CurrentPrefab.transform.GetChild(1).GetComponent<TMP_Text>();
        CurrentLVandName = CurrentPrefab.transform.GetChild(2).GetComponent<TMP_Text>();
        CurrentSlider.maxValue = _MonsterStatus.max_hp;
        CurrentSlider.value = currentHP;
        CurrentHPText.text = currentHP.ToString();
        CurrentLVandName.text = "LV" + _MonsterStatus.lv + " " + _MonsterStatus.MonsterName;
        CurrentPrefab.transform.position = MainCamera.WorldToScreenPoint(UIDisplaySpot.position);
        Using_UIPrefab = true;
    }

    void UpdateUIPos()
    {
        if (Using_UIPrefab)
        {
            CurrentPrefab.transform.position = MainCamera.WorldToScreenPoint(UIDisplaySpot.position);
        }
    }
    void UpdateUIStatus()
    {
        if (Using_UIPrefab)
        {
            CurrentSlider.value = currentHP;
            CurrentHPText.text = currentHP.ToString();
        }
    }

    public void DestroyDisplayUI() // When Enemy is too far or not in attack mode.
    {
        if (Using_UIPrefab)
        {
            Destroy(CurrentPrefab);
            Using_UIPrefab = false;
        }
    }

}
