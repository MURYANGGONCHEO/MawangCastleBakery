using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Handling : MonoBehaviour
{
    [SerializeField] private Transform _handTrm;

    [Header("핸드 포스")]
    [SerializeField] private float _normalHandYPos;
    [SerializeField] private float _downHandYPos;

    public void MoveHand(bool isDown)
    {
        float targetYPos = !isDown ? _downHandYPos : _normalHandYPos;
        _handTrm.DOLocalMoveY(targetYPos, 0.2f);
    }
}
