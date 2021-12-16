using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MonsterStatus", menuName = "MonsterStatus")]
public class MonsterStatus : ScriptableObject
{
    public int lv; 
    public int max_hp;
    //public int max_mp;
    public int attack;
    public int defense;
    public int Drop_Exp = 10;
    public int DropMoney = 10;
    public CardData DropCard; // when the monster dead, will drop this card.

}
