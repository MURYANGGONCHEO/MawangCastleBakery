using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAttackBuff : SpecialBuff, IOnTakeDamage
{
    private List<Health> appliedEnemy = new();
    private bool active = false;

    public void TakeDamage(Health health,ref int dmg)
    {
        active = true;

        if (appliedEnemy.Contains(health)) return;
        appliedEnemy.Add(health);
        if (!active) BattleReader.SkillCardManagement.useCardEndEvnet.AddListener(EndAttack);

        health.AilmentStat.ApplyAilments(AilmentEnum.Chilled);
    }

    private void EndAttack()
    {
        SetIsComplete(true);
    }

    public override void SetIsComplete(bool value)
    {
        base.SetIsComplete(value);
        if(value)
            BattleReader.SkillCardManagement.useCardEndEvnet.RemoveListener(EndAttack);
    }
}
