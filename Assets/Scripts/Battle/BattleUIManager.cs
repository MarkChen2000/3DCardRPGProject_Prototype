using System.Collections;
using UnityEngine;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    public GameObject PopupDamageValue_Prefab;

    GameObject Panel_UpStatusUI;
    Transform Trans_DisplayUICanvas;
    Camera MainCamera;
    
    public float EnemyPopupValueUI_LifeTime = 1f;

    void Awake()
    {
        Panel_UpStatusUI = transform.GetChild(0).GetChild(0).gameObject;
        Trans_DisplayUICanvas = transform.GetChild(1).GetChild(0);
        MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    public void SwitchUpStatusUIOnOff(bool OnOff)
    {
        Panel_UpStatusUI.SetActive(OnOff);
    }

    public void SpawnPopupDamageUI(Vector2 damageinfo,Transform UIDisplaySpot)
    {
        GameObject prefab = Instantiate(PopupDamageValue_Prefab, Trans_DisplayUICanvas);
        TMP_Text tmp_text = prefab.transform.GetChild(0).GetComponent<TMP_Text>();
        tmp_text.text = damageinfo.y.ToString();
        if (damageinfo.x != 1) // mean this hit is not critical.
        {
            Destroy(prefab.transform.GetChild(1).gameObject);
        }
        prefab.transform.position = MainCamera.WorldToScreenPoint(UIDisplaySpot.position);
        StartCoroutine(FadeoutPopupDamageUI(prefab));
    }

    IEnumerator FadeoutPopupDamageUI(GameObject prefab)
    {
        CanvasGroup cg = prefab.GetComponent<CanvasGroup>();
        while (cg.alpha > 0)
        {
            cg.alpha -= 1 / EnemyPopupValueUI_LifeTime * Time.unscaledDeltaTime;
            yield return null;
        }
        Destroy(prefab);
    }
}
