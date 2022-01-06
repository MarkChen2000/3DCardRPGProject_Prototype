using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntireInventoryController : MonoBehaviour
{
    private CharacterBasicAttackController PlayerBasciAttack_Con;

    private InventoryController InventoryCon;
    private EquipmentSlotController EquipmentCon;
    private CardDeckController DeckCon;
    //private CardBattleController CardBattleCon;

    private GameObject Panel_DisableInventoryBarrier;
    private Canvas Canvas_Inventory;

    private void Awake()
    {
        PlayerBasciAttack_Con = GameObject.Find("Player").GetComponent<CharacterBasicAttackController>();

        InventoryCon = GetComponent<InventoryController>();
        EquipmentCon = GetComponent<EquipmentSlotController>();
        DeckCon = GetComponent<CardDeckController>();
        //CardBattleCon = GameObject.Find("BattleUI").GetComponent<CardBattleController>();

        Panel_DisableInventoryBarrier = transform.GetChild(0).GetChild(1).gameObject;
        Canvas_Inventory = transform.GetChild(0).GetComponent<Canvas>();

        SaveandLoadEntireInvData(true); // Load
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchInventoryOnOff(false);
        SwitchInventoryBarrierOnOff(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            if (Canvas_Inventory.enabled == true)
            {
                SwitchInventoryOnOff(false);
            }
            else
            {
                SwitchInventoryOnOff(true);
            }
        }
    }

    public void SaveandLoadEntireInvData( bool SorL ) 
        // inventor card, deck, equipmentslot will need to load in carddata in the Start function,
        // so this function should be called in Awake function. 
    {
        InventoryCon.SaveandLoadInvCardList(SorL);
        EquipmentCon.SaveandLoadEquipSlot(SorL);
        DeckCon.SaveandLoadCardDeck(SorL);
    }

    private void SwitchInventoryOnOff(bool OnOff) // true mean on, false mean off.
    // TurnOn or Off the Inventory UI.
    // Because sometime stil need inventory hierachy outside the inventory, so can't simplely disactive Inventory panel.
    // But turn on or off canvas still cost some performance.
    {
        Canvas_Inventory.enabled = OnOff;
        //CardBattleCon.Activate(!OnOff);
    }

    public void SwitchInventoryBarrierOnOff(bool OnOff)
    {
        Panel_DisableInventoryBarrier.SetActive(OnOff);
    }

}
