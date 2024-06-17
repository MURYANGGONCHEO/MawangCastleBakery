using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MineBattlePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Transform _scalingTrm;
    [SerializeField] private UnityEvent _mineGoBattleEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        _mineGoBattleEvent?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _scalingTrm.DOKill();
        _scalingTrm.DOScale(1.1f, 0.1f).SetEase(Ease.OutBounce);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _scalingTrm.DOKill();
        _scalingTrm.DOScale(1f, 0.1f);
    }
}
