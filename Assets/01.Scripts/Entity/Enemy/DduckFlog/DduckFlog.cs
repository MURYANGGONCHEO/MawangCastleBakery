using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DduckFlog : Enemy
{
    private int rollAnimHash = Animator.StringToHash("roll");
    protected override void Awake()
    {
        base.Awake();
    }
    public override void Attack()
    {
        base.Attack();
        OnAttackStart?.Invoke();
        MoveToTargetForward(Vector3.zero);
    }

    public override void SlowEntityBy(float percent)
    {
    }

    public override void TurnStart()
    {
        base.TurnStart();
        OnAnimationCall += AttackAnimationCall;
    }
    public void AttackAnimationCall()
    {
        AnimatorCompo.SetBool(rollAnimHash, true);
    }
    public override void TurnAction()
    {
        turnStatus = TurnStatus.Running;
        Attack();
    }
    public override void TurnEnd()
    {
        OnAnimationCall -= AttackAnimationCall;
        base.TurnEnd();
    }

    public override void MoveToTargetForward(Vector3 pos)
    {
        lastMovePos = transform.position;


        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOJump(target.transform.position, 1, 1, 0.6f));
        seq.OnComplete(OnMoveTarget.Invoke);
    }
    public override void MoveToOriginPos()
    {
        transform.DOJump(lastMovePos, 1, 1, 0.6f).OnComplete(OnMoveOriginPos.Invoke);
    }
    protected override void HandleEndMoveToOriginPos()
    {
        OnAttackEnd?.Invoke();
        turnStatus = TurnStatus.End;
    }

    protected override void HandleEndMoveToTarget()
    {
        AnimatorCompo.SetBool(rollAnimHash, false);
        target.HealthCompo.ApplyDamage(CharStat.GetDamage(), this);
        MoveToOriginPos();
    }
}
