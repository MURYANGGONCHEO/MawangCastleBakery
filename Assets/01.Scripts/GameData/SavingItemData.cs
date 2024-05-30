using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemData
{
    public string itmeName;
    public int itemCount;
}

public class SavingItemData : CanSaveData
{
    public List<ItemData> itemDataList = new ();

    public override void SetInitialValue()
    {
    }
}
