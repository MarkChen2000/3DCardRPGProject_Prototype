using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardListType { BattleCardList, InventoryCrdList }

[CreateAssetMenu(fileName = "New CardList", menuName = "Inventory/CardList")]
public class CardList : ScriptableObject
{
    public string CardListName = "DefaultCardListName";

    public CardListType _CradListType;

    public List<CardData> _CardList = new List<CardData>();

    // 12.12 note:
    // Because of some mistake, we have to cancle these scriptable object function.
    // By orgin thought, we thought these SO can do the storage function and take/change these variables in it,
    // but now these SO should only have "setting initial value" function for now on,
    // and load in their value to controller at game start,
    // so we move these functions to their own controller.

    void OnEnable() // when this SO get refernece, sort and remove empty element in list first.
    {
        SortListinAsset();
        RemoveAllEmptyElement();
    }

    private void SortListinAsset() // Sort the cards in the list.
    {
        if (_CardList.Count == 0)
        {
            Debug.Log("Didnt have card inside the CardLIst !");
        }
        else
        {
            // Because Carddata-type data is not the regular type like int or string that can be sorted defaulty.
            // Have to use "IComparble" to compare the data (CardCost) of elements in list, with self-made sort function.
            _CardList.Sort(); // Sort by the Cost of Card.
        }
    }

    private void RemoveAllEmptyElement()
    {
        for (int i = 0; i < _CardList.Count; i++)
        {
            if (_CardList[i] == null)
            {
                _CardList.Remove(_CardList[i]);
                Debug.Log("Element in _CardList[" + i + "] is empty! Removing the empty element.");
            }
        }
    }

    /*private int GetDiffCardNum() // Get how many DIFFERENT Card in the list;
    {
        if (_CardList.Count == 0) return 0;

        SortListinAsset();
        int differnrtcount = 1;
        for (int i = 0; i < _CardList.Count - 1; i++)
        {
            if (_CardList[i] != _CardList[i + 1])
            {
                differnrtcount++;
            }
        }
        return differnrtcount;
    }*/

    /*private int getLength()
    {
        return _CardList.Count;
    }*/

}
