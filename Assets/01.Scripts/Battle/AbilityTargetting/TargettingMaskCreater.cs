using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingMaskCreater : MonoBehaviour
{
    private Dictionary<Enemy, TargetMask> _getTargetMaskDic = new Dictionary<Enemy, TargetMask>();  
    public Dictionary<Enemy, TargetMask> GetTargetMaskDic => _getTargetMaskDic;
    [SerializeField] private TargetMask _targetMaskRect;

    public void CreateMask(Enemy enemy, Vector2 enemyPos)
    {
        TargetMask tm = Instantiate(_targetMaskRect, transform);
        tm.MarkingEnemy = enemy;
        RectTransform rt = tm.transform as RectTransform;
        
        Vector3 pos = enemyPos;
        pos.z = 0;
        rt.position = pos;

        rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y, 0);

        _getTargetMaskDic.Add(enemy, tm);
    }

    public void MaskDown(Enemy enemy)
    {
        if(enemy == null) return;

        GetTargetMaskDic[enemy].enabled = false;
    }

    public void MaskUp(Enemy enemy)
    {
        if (enemy == null) return;

        GetTargetMaskDic[enemy].enabled = true;
    }
}
