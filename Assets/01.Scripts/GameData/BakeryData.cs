using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CakeData
{
    public string CakeName { get; private set; }
    public bool IsFavorites { get; set; }
    public int Count { get; set; } = 1;
public struct CakeData
{
    public string CakeName;
    public bool IsFavorites;

    public CakeData(string _cakeName,  bool _isFavorites)
    {
        CakeName = _cakeName;
        IsFavorites = _isFavorites;
    }
}

public class BakeryData : CanSaveData
{
    public List<CakeData> CakeDataList = new List<CakeData>();

    public override void SetInitialValue()
    {

    }
}
