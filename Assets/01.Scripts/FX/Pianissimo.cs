using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pianissimo : MonoBehaviour
{
    public Transform target;
    private float _speed = 0.0f;
    private Vector3 _dir;

    private void OnEnable()
    {

    }

    private void Update()
    {
        transform.position += _dir * _speed;
    }

    public void Ready()
    {
        _dir = (target.position - transform.position).normalized;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DORotate(_dir , 0.4f));
        seq.AppendCallback(() =>
        {
            Attack();
        });
    }

    private void Attack()
    {
        _speed = 0.2f;
    }
}
