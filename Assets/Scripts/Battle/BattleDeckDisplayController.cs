using UnityEngine;
using System.Collections.Generic;

public class BattleDeckDisplayController : MonoBehaviour
{
    CardBattleController CardBattleCon;

    public GameObject BattleDeckTemplatePrefab;

    Transform Trans_BattleDeckPanel;
    Transform Trans_DeckCardGridGroup;





    // This script is too difficult to finish right now.






    /*void Awake()
    {
        CardBattleCon = GetComponent<CardBattleController>();

        Trans_BattleDeckPanel = transform.GetChild(0).GetChild(2);
        Trans_DeckCardGridGroup = Trans_BattleDeckPanel.GetChild(0);
    }*/

    public void SwitchBattleDeckDisplay(bool OnOff)
    {
        Trans_BattleDeckPanel.gameObject.SetActive(OnOff);
    }

    public void SpawnTemplateAndDisplay()
    {
        for (int i = 0; i < CardBattleCon.BattleCardList.Count ; i++)
        {
            DeckCard_DataLoaderAndDisplay template = Instantiate(BattleDeckTemplatePrefab, Trans_DeckCardGridGroup).GetComponent<DeckCard_DataLoaderAndDisplay>();
            template.DisplaytoTemplate(CardBattleCon.BattleCardList[i]);
        }
    }


}
