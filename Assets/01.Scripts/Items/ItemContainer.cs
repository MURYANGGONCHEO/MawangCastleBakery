using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    public Dictionary<string, ItemDataSO> ItemDataDic { get; private set; } = new();
    [SerializeField] private ItemDataSO[] _itemDataArr;

    private void Awake()
    {
        foreach (var itemData in _itemDataArr)
        {
            ItemDataDic.Add(itemData.itemName, itemData);
        }
    }
}
