using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiwiBird : Enemy
{
    private ThrowController _throwKiwi;
    private int _animCatchHash = Animator.StringToHash("catchKiwi");
    [SerializeField]private Transform kiwiSpawnTrm;

    public override void Attack()
    {
        OnAttackStart?.Invoke();
        
    }

    private void CatchKiwi()
    {
        AnimatorCompo.SetTrigger(_animCatchHash);
        PoolManager.Instance.Push(_throwKiwi);
        turnStatus = TurnStatus.End;
        OnAttackEnd?.Invoke();
    }
    private void ThrowKiwi()
    {
        _throwKiwi = PoolManager.Instance.Pop(PoolingType.ThrowKiwi) as ThrowController;
        _throwKiwi.transform.position = kiwiSpawnTrm.position;
        _throwKiwi.Throw(this, target, CatchKiwi);
    }

    public override void SlowEntityBy(float percent)
    { 
    }
    public override void TurnStart()
    {
        base.TurnStart();
        OnAnimationCall += ThrowKiwi;
    }

    public override void TurnAction()
    {
        Attack();
    }
    public override void TurnEnd()
    {
        OnAnimationCall -= ThrowKiwi;
        base.TurnEnd();
    }
    protected override void HandleEndMoveToOriginPos()
    {
    }

    protected override void HandleEndMoveToTarget()
    {
    }
}
