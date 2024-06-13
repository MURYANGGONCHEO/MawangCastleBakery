using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pianissimo : MonoBehaviour
{
    public Transform target;
    private float _speed = 0.0f;

    private void OnEnable()
    {

    }

    private void Update()
    {
        transform.position += transform.forward * _speed;
    }

    public void Ready()
    {
        Vector2 dir = (target.position - transform.position).normalized;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DORotate(dir , 0.4f));
        seq.AppendCallback(() =>
        {
            Attack();
        });
    }

    private void Attack()
    {
        _speed = 0.5f;
    }
}
