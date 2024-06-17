using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrMuddy : Enemy
{
    protected override void Start()
    {
        base.Start();
    }

    public override void Attack()
    {
        target.HealthCompo.ApplyDamage(CharStat.GetDamage(),this);
        base.Attack();
        //VFXPlayer.PlayHitEffect(attackParticle, target.transform.position);
        MoveToOriginPos();
        OnAttackStart?.Invoke();
        //OnAttackEnd?.Invoke();
    }

    public override void SlowEntityBy(float percent)
    {
    }

    public override void TurnAction()
    {
        turnStatus = TurnStatus.Running;
        AnimatorCompo.SetBool(attackAnimationHash,true);

        MoveToTargetForward(Vector3.zero);
    }

    public override void TurnEnd()
    {
        base.TurnEnd();
    }

    public override void TurnStart()
    {
        base.TurnStart();
        OnAttackEnd?.Invoke();

    }

    protected override void HandleEndMoveToOriginPos()
    {
        turnStatus = TurnStatus.End;
    }

    protected override void HandleEndMoveToTarget()
    {
        Attack();
    }
}
