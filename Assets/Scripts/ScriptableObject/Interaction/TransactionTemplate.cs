using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transaction Template", menuName = "InteractionTemplate/Transaction")]
public class TransactionTemplate : ScriptableObject
{
    public string ShopKeeperName = "ShopKeeperA" ;
    public string PackName = "TestingPack";
    public int CostofOnePack = 100;
    public CardList PackCardPoolList ;

    [TextArea, Tooltip("Saying these text, before get into the Shop system")]
    public List<string> BeforeShopSystemTextList = new List<string>();
    [TextArea, Tooltip("Saying these text, before leaving the Shop system")]
    public List<string> LeavingShopSystemTextList = new List<string>();

    [Tooltip("The cards sell price with multiply by this value.")]
    public float SellPriceMultiplier = 1f;
    [Tooltip("The cards buying price will multiply by this value.")]
    public float BuyPriceMultiplier = 0.5f;
}
