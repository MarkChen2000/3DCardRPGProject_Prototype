using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transaction Template", menuName = "InteractTemplate/Transaction")]
public class TransactionTemplate : ScriptableObject
{
    public string ShopKeeperName;
    public List<CardData> ShopContainCardList = new List<CardData>();
}
