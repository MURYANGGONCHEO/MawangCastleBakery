using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BuffingType
{
    Targetting,
    Healing,
    AtkUp,
    DefUp,
    CriPercentUp,
    CriDamageUp,
    Frozen,
    Lightning,
    Faint,
    AtkDown,
    DefDown
}

public struct CombatMarkingData
{
    public BuffingType buffingType;
    public string buffingInfo;

    public CombatMarkingData(BuffingType buffType, string buffInfo)
    {
        buffingType = buffType;
        buffingInfo = buffInfo;
    }
}

public class BuffingMarkSetter : MonoBehaviour
{
    [SerializeField] private BuffingMark _buffingMarkPrefab;
    [SerializeField] private Vector2 _startBuffingPosition;
    [SerializeField] private float _buffingMarkDistance;
    [SerializeField] private Sprite[] _buffingMarkTextureArr;
    [SerializeField] private string[] _buffingNameArr;

    private List<BuffingMark> _buffingMarkList = new List<BuffingMark>();

    public void AddBuffingMark(CombatMarkingData markingData)
    {
        BuffingMark buffingMark = Instantiate(_buffingMarkPrefab, transform);

        int idx = (int)markingData.buffingType;
        buffingMark.SetInfo(_buffingMarkTextureArr[idx], 
                            _buffingNameArr[idx], markingData);

        BuffingMarkPositionSetter(buffingMark.transform);
        _buffingMarkList.Add(buffingMark);
    }

    public void RemoveBuffingMark(CombatMarkingData markingData)
    {
        BuffingMark target = _buffingMarkList.Find(x => x.CombatMarkingData.Equals(markingData));
        int idx = _buffingMarkList.IndexOf(target);
        _buffingMarkList.Remove(target);

        Destroy(target.gameObject);
        BuffingMarkPositionGenerate(idx);
    }

    private void BuffingMarkPositionSetter(Transform buffingMarkTrm)
    {
        buffingMarkTrm.localPosition =
        new Vector2(_startBuffingPosition.x - (_buffingMarkDistance * _buffingMarkList.Count),
                    _startBuffingPosition.y + 18);

        buffingMarkTrm.DOLocalMoveY(buffingMarkTrm.localPosition.y - 18, 0.2f).OnComplete(() =>
        {
            if (_buffingMarkList.Count <= 3) return;

            foreach(var item in _buffingMarkList)
            {
                item.transform.DOLocalMoveX(item.transform.localPosition.x + 20, 0.1f).SetEase(Ease.OutBounce);
            }
        });
    }

    private void BuffingMarkPositionGenerate(int deleteIdx)
    {
        for(int i = 0; i <  _buffingMarkList.Count; i++)
        {
            float targetX = _buffingMarkList[i].transform.localPosition.x;
            if (i < deleteIdx)
            {
                targetX += 20;
            }
            else
            {
                targetX -= 20;
            }

            _buffingMarkList[i].transform.DOLocalMoveX(targetX, 0.1f).SetEase(Ease.OutBounce);
        }
    }
}
