using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chameleon : Enemy
{
    private ThrowController _throwMelon;
    [SerializeField] private Transform melonSpawnTrm;

    public override void Attack()
    {
        OnAttackStart?.Invoke();
    }

    private void ThrowMelon()
    {
        _throwMelon = PoolManager.Instance.Pop(PoolingType.ThrowMelon) as ThrowController;
        _throwMelon.transform.position = melonSpawnTrm.position;
        _throwMelon.Throw(this, target, AttackEnd);
    }

    private void AttackEnd()
    {
        PoolManager.Instance.Push(_throwMelon);
        turnStatus = TurnStatus.End;
        OnAttackEnd?.Invoke();
    }

    public override void SlowEntityBy(float percent)
    {
    }
    public override void TurnStart()
    {
        base.TurnStart();
        OnAnimationCall += ThrowMelon;
    }

    public override void TurnAction()
    {
        Attack();
    }
    public override void TurnEnd()
    {
        OnAnimationCall -= ThrowMelon;
        base.TurnEnd();
    }
    protected override void HandleEndMoveToOriginPos()
    {
    }

    protected override void HandleEndMoveToTarget()
    {
    }
}
