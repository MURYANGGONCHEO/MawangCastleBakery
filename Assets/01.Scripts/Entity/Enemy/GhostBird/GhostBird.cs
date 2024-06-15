using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GhostBird : Enemy
{
    public override void Attack()
    {
        OnAttackStart?.Invoke();
        MoveToTargetForward(target.forwardTrm.position);
    }

    public override void SlowEntityBy(float percent)
    {
    }

    public override void TurnAction()
    {
        print("¿ÖÁö");
        Attack();
    }

    public override void MoveToTargetForward(Vector3 pos)
    {
        lastMovePos = transform.position;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(pos + Vector3.up * 5f, 1.2f));
        seq.Append(transform.DOMove(pos + Vector3.up * 5.2f, 0.25f));
        seq.AppendCallback(() => AnimatorCompo.SetTrigger(attackTriggerAnimationHash));
        seq.Append(transform.DOMove(pos, 0.05f)).SetEase(Ease.Unset);
        seq.OnComplete(OnMoveTarget.Invoke);
    }

    protected override void HandleEndMoveToOriginPos()
    {
        turnStatus = TurnStatus.End;
        OnAttackEnd?.Invoke();
    }

    protected override void HandleEndMoveToTarget()
    {
        target.HealthCompo.ApplyDamage(CharStat.GetDamage(), this);
        AnimatorCompo.SetBool(attackAnimationHash,false);
        MoveToOriginPos();
    }
}
