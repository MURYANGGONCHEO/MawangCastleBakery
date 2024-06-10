using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingButterDog : Enemy
{
    protected override void Start() 
    {
        base.Start();
    }
    public override void Attack()
    {
        target.HealthCompo.ApplyDamage(CharStat.GetDamage(), this);
        //VFXPlayer.PlayHitEffect(attackParticle, target.transform.position);
    }

    public override void SlowEntityBy(float percent)
    {
    }

    public override void TurnAction()
    {
        OnAttackStart?.Invoke();
        MoveToTargetForward(Vector3.zero);
    }

    public override void MoveToTargetForward(Vector3 p)
    {
        StartCoroutine(AttackCor());

        Sequence seq = DOTween.Sequence();

        seq.OnComplete(OnMoveTarget.Invoke);
    }

    private IEnumerator AttackCor()
    {
        yield return new WaitForSeconds(0.8f);
        Attack();
        OnAttackEnd?.Invoke();
    }

    public override void Spawn(Vector3 spawnPos, Action callBack)
    {
        SpriteRendererCompo.material.SetFloat("_dissolve_amount", 0);

        AnimatorCompo.SetBool(spawnAnimationHash, true);

        transform.position = spawnPos + new Vector3(-20f, 0);
        transform.DOMoveX(spawnPos.x, 1f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            AnimatorCompo.SetBool(spawnAnimationHash, false);
            turnStatus = TurnStatus.Ready;
            callBack?.Invoke();
        });
    }
    public override void MoveToOriginPos()
    {
        transform.DOMove(lastMovePos, 0.5f).SetEase(Ease.Linear).OnComplete(OnMoveOriginPos.Invoke);
    }


    protected override void HandleEndMoveToOriginPos()
    {
        turnStatus = TurnStatus.End;
    }

    protected override void HandleEndMoveToTarget()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector2(-30, screenPos.y));
        MoveToOriginPos();
    }
}
