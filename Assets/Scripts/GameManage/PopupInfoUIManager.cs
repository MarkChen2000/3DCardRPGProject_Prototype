using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupInfoUIManager : MonoBehaviour
{
    Transform Trans_PopupInfoDisplay;
    public GameObject PopupInfo_Prefab;

    public float PopupInfo_LifeTime = 2f;

    private void Awake()
    {
        Trans_PopupInfoDisplay = transform.GetChild(0).GetChild(0);
    }

    public void SpawnPopupInfoUI(string content)
    {
        GameObject prefab = Instantiate(PopupInfo_Prefab, Trans_PopupInfoDisplay);
        prefab.transform.GetChild(1).GetComponent<TMP_Text>().text = content ;
        Destroy(prefab, PopupInfo_LifeTime);
    }
    


}
