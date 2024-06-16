using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonStrowberry : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        VFXPlayer.OnEndEffect += () =>
        {
            turnStatus = TurnStatus.End;
            OnAttackEnd?.Invoke();
        };
    }

    public override void Attack()
    {
        attackParticle.attack.AddTriggerTarget(target);

        OnAttackStart?.Invoke();
        Vector3 pos = (Vector3)attackParticle.attack.transform.position - new Vector3(1.52f, 0);
        Vector3 dir = (Vector3)target.transform.position - pos;

        attackParticle.attack.transform.right = -dir;
        VFXPlayer.PlayParticle(attackParticle);
    }

    public override void SlowEntityBy(float percent)
    {
    }

    public override void TurnAction()
    {
        turnStatus = TurnStatus.Running;
        Attack();
    }

    public override void TurnEnd()
    {
        OnAttackEnd?.Invoke();
        base.TurnEnd();
    }

    public override void TurnStart()
    {
        base.TurnStart();
        turnStatus = TurnStatus.Ready;
    }

    protected override void HandleEndMoveToOriginPos()
    {
    }

    protected override void HandleEndMoveToTarget()
    {
    }
}
