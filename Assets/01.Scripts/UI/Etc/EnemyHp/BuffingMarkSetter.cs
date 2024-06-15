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
    DefDown,
    Smelting
}

public class CombatMarkingData
{
    public BuffingType buffingType;
    public string buffingInfo;
    public int durationTurn;

    public CombatMarkingData(BuffingType buffType, string buffInfo, int duration)
    {
        buffingType = buffType;
        buffingInfo = buffInfo;
        durationTurn = Mathf.Clamp(duration, 1, int.MaxValue);
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
    public Transform BuffingPanelTrm { get; set; }

    private Tween[] _movingTweenArr;

    private void Start()
    {
        _movingTweenArr = new Tween[_buffingMarkList.Count];
        TurnCounter.EnemyTurnEndEvent += DecountBuffDuration;
    }
    private void OnDestroy()
    {
        TurnCounter.EnemyTurnEndEvent -= DecountBuffDuration;
    }

    public void DecountBuffDuration()
    {
        List<BuffingMark> disappearList = new();
        foreach(var bm in _buffingMarkList)
        {
            int turnCount = --bm.CombatMarkingData.durationTurn;
            if(turnCount == 0)
            {
                disappearList.Add(bm);
            }
        }

        foreach (var bm in disappearList)
        {
            RemoveBuffingMark(bm.CombatMarkingData);
        }
    }

    public void AddBuffingMark(CombatMarkingData markingData)
    {
        BuffingMark buffingMark = Instantiate(_buffingMarkPrefab, transform);

        int idx = (int)markingData.buffingType;
        buffingMark.SetInfo(_buffingMarkTextureArr[idx], 
                            _buffingNameArr[idx], markingData, BuffingPanelTrm);

        _buffingMarkList.Add(buffingMark);
        BuffingMarkPositionSetter(buffingMark.transform);
    }

    public void RemoveBuffingMark(CombatMarkingData markingData)
    {
        BuffingMark target = _buffingMarkList.Find(x => x.CombatMarkingData.Equals(markingData));
        if (target == null) return;

        int idx = _buffingMarkList.IndexOf(target);
        _buffingMarkList.Remove(target);

        Destroy(target.gameObject);
        BuffingMarkPositionGenerate();
    }

    public void RemoveSpecificBuffingType(BuffingType buffingType)
    {
        BuffingMark[] markArr =
        _buffingMarkList.FindAll(x => x.CombatMarkingData.buffingType == buffingType).ToArray();

        foreach(var bf in markArr)
        {
            RemoveBuffingMark(bf.CombatMarkingData);
        }
    }

    private void BuffingMarkPositionSetter(Transform buffingMarkTrm)
    {
        bool isMovingX = _buffingMarkList.Count > 3;

        float x = !isMovingX ?
        (_startBuffingPosition.x - (_buffingMarkDistance * (_buffingMarkList.Count - 1))) :
        _buffingMarkList[_buffingMarkList.Count - 2].transform.localPosition.x - _buffingMarkDistance;

        buffingMarkTrm.localPosition =
        new Vector2(x, _startBuffingPosition.y + 18);

        buffingMarkTrm.DOLocalMoveY(buffingMarkTrm.localPosition.y - 18, 0.2f).OnComplete(() =>
        {
            if (isMovingX)
            {
                foreach (var item in _buffingMarkList)
                {
                    item.transform.DOLocalMoveX(item.transform.localPosition.x + 20, 0.1f).SetEase(Ease.OutBounce);
                }
            }
            
        });
    }

    private void BuffingMarkPositionGenerate()
    {
        foreach(Tween t in _movingTweenArr)
        {
            t.Kill();
        }

        float[] arr = new float[_buffingMarkList.Count];
        _movingTweenArr = new Tween[_buffingMarkList.Count];
        int temp = arr.Length > 3 ? 20 * (arr.Length - 3) : 0;

        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = _startBuffingPosition.x - (_buffingMarkDistance * i) + temp;
        }

        for(int i = 0; i <  _buffingMarkList.Count; i++)
        {
            _movingTweenArr[i] = 
            _buffingMarkList[i].transform.DOLocalMoveX(arr[i], 0.1f).SetEase(Ease.OutBounce);
        }
    }
}
