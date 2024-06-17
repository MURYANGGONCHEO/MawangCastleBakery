using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingFace : MonoBehaviour
{
    [SerializeField] private float _floatingValue;

    private void Start()
    {
        float time = Random.Range(1f, 3f);

        float normalY = transform.localPosition.y;
        Tween t = transform.DOLocalMoveY(normalY + _floatingValue, time).OnComplete(() =>
                  transform.DOLocalMoveY(normalY, time));

        t.SetLoops(-1, LoopType.Yoyo);
    }
}
