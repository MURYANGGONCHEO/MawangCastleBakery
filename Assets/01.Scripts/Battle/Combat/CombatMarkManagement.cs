using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatMarkManagement : MonoBehaviour 
{
    private Dictionary<int, List<CombatMarkingData>> _markingDataDic = new();

    public void AddBuffingData(Entity entity, int cardID, CombatMarkingData markingData, int count = 1)
    {
        if(entity.HealthCompo.IsDead) return;

        for(int i = 0; i< count; i++)
        {
            entity.BuffSetter.AddBuffingMark(markingData);

            if (!_markingDataDic.ContainsKey(cardID))
            {
                _markingDataDic.Add(cardID, new List<CombatMarkingData>());
            }
            _markingDataDic[cardID].Add(markingData);
        }
    }

    public void RemoveBuffingData(Entity entity, int cardID, BuffingType markingType, int count = 1)
    {
        CombatMarkingData data =
        _markingDataDic[cardID].FirstOrDefault(x => x.buffingType == markingType);

        if (data == null)
        {
            Debug.LogError($"Error : Cant Remove : {markingType}Data, It dose not exist");
            return;
        }

        entity.BuffSetter.RemoveBuffingMark(data, count);
        _markingDataDic[cardID].Remove(data);
    }
}
