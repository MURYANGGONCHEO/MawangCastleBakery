using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    private Dictionary<string, ItemDataSO> _itemDataDic = new();
    [SerializeField] private ItemDataSO[] _itemDataArr;

    private void Awake()
    {
        foreach (var itemData in _itemDataArr)
        {
            _itemDataDic.Add(itemData.itemName, itemData);
        }
    }

    public ItemDataSO GetItemDataByName(string name)
    {
        return _itemDataDic[name];
    }
}
