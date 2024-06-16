using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pianissimo : MonoBehaviour
{
    public Transform target;
    private float _speed = 0.0f;
    private Vector3 _dir;

    public bool isTriggered = false;

    private void Update()
    {
        if (!isTriggered)
            transform.position += _dir * _speed;
    }

    public void Ready()
    {
        _dir = (target.position - transform.position).normalized;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DORotate(_dir, 1f).SetEase(Ease.OutExpo));
        seq.AppendCallback(() =>
        {
            Attack();
        });
    }

    private void Attack()
    {
        _speed = 0.7f;
    }
}
