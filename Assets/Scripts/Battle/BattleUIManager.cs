using System.Collections;
using UnityEngine;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    public GameObject PopupDamageValueUI_Prefab;
    public GameObject PopupDropValueUI_Prefab;
    public GameObject PopupHandCardUI_Prefab;

    GameObject Panel_UpStatusUI;
    Transform Trans_EnemyStatusDisplayUICanvas; 
    Transform Trans_HandCardInfoDisplayUICanvas; // the different between these two canvas is that
                                                 // one is display in front of hand card, another one is behind hand card.
    Camera MainCamera;
    
    public float EnemyPopupValueUI_LifeTime = 1f;
    public float HandCardInfo_LifeTime = 2f;

    void Awake()
    {
        Panel_UpStatusUI = transform.GetChild(0).GetChild(0).gameObject;
        Trans_EnemyStatusDisplayUICanvas = transform.GetChild(1).GetChild(0);
        Trans_HandCardInfoDisplayUICanvas = transform.GetChild(2).GetChild(0);
        MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    public void SwitchUpStatusUIOnOff(bool OnOff)
    {
        Panel_UpStatusUI.SetActive(OnOff);
    }

    public void SpawnPopupDamageUI(Vector2 damageinfo,Transform UIDisplaySpot)
    {
        GameObject prefab = Instantiate(PopupDamageValueUI_Prefab, Trans_EnemyStatusDisplayUICanvas);
        TMP_Text tmp_text = prefab.transform.GetChild(0).GetComponent<TMP_Text>();
        tmp_text.text = damageinfo.y.ToString();
        if (damageinfo.x != 1) // mean this hit is not critical.
        {
            Destroy(prefab.transform.GetChild(1).gameObject);
        }
        prefab.transform.position = MainCamera.WorldToScreenPoint(UIDisplaySpot.position);
        StartCoroutine(FadeoutPopupUI(prefab,EnemyPopupValueUI_LifeTime));
    }

    public void SpawnPopupDropUI(int money, int exp, Transform UIDisplaySpot)
    {
        GameObject prefab = Instantiate(PopupDropValueUI_Prefab, Trans_EnemyStatusDisplayUICanvas);
        TMP_Text tmp_money = prefab.transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text tmp_exp = prefab.transform.GetChild(1).GetComponent<TMP_Text>();
        tmp_money.text = money.ToString() + "$";
        tmp_exp.text = "Exp: " + exp.ToString();

        Vector3 adjustpos = MainCamera.WorldToScreenPoint(UIDisplaySpot.position); // For preventing that damage value and drop value will stick together.
        adjustpos.y += 30f;
        adjustpos.x -= 30f;
        prefab.transform.position = adjustpos;
        StartCoroutine(FadeoutPopupUI(prefab,EnemyPopupValueUI_LifeTime));
    }

    public void SpawnHandCardPopupInfo(string content, Vector3 UIDisplayPoint)
    {
        //Debug.Log(content);

        GameObject prefab = Instantiate(PopupHandCardUI_Prefab, Trans_HandCardInfoDisplayUICanvas);
        prefab.transform.GetChild(0).GetComponent<TMP_Text>().text = content;
        prefab.transform.position = MainCamera.WorldToScreenPoint(UIDisplayPoint);
        StartCoroutine(FadeoutPopupUI(prefab, HandCardInfo_LifeTime));
    }

    IEnumerator FadeoutPopupUI(GameObject prefab, float lifetime)
    {
        CanvasGroup cg = prefab.GetComponent<CanvasGroup>();
        while (cg.alpha > 0)
        {
            cg.alpha -= 1 / lifetime * Time.unscaledDeltaTime;
            yield return null;
        }
        Destroy(prefab);
    }

}
