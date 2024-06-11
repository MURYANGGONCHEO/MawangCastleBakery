using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : KnockBackBase
{
    public KnockBack(KnockBackSystem system, Entity entity) : base(system, entity)
    {
    }

    public override void Knockback(int dmg)
    {
        Vector3 knockbackPos = _owner.transform.position;
        knockbackPos.x += (_system.filpX ? -1 : 1) * dmg * _system.knockBackAmount;
        _owner.transform.DOMove(knockbackPos, 0.025f).SetEase(Ease.OutQuad);
    }
}
