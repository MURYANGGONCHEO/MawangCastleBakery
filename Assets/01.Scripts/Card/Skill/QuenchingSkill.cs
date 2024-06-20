using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuenchingSkill : CardBase, ISkillEffectAnim
{
    public override void Abillity()
    {
        IsActivingAbillity = true;

        Player.UseAbility(this);
        Player.OnAnimationCall += HandleAnimationCall;
        Player.VFXManager.OnEndEffectEvent += HandleEffectEnd;
    }

    public void HandleAnimationCall()
    {
        SoundManager.PlayAudio(_soundEffect, true);
        Player.VFXManager.PlayParticle(CardInfo, (int)CombineLevel, _skillDurations[(int)CombineLevel]);
        StartCoroutine(SkillCor());
        Player.OnAnimationCall -= HandleAnimationCall;
    }

    public void HandleEffectEnd()
    {
        Player.EndAbility();
        Player.VFXManager.EndParticle(CardInfo, (int)CombineLevel);
        IsActivingAbillity = false;
        Player.VFXManager.OnEndEffectEvent -= HandleEffectEnd;
    }

    private IEnumerator SkillCor()
    {
        yield return new WaitForSeconds(0.3f);

        Player.BuffStatCompo.AddStack(StackEnum.Forging, buffSO.stackBuffs[0].values[(int)CombineLevel]);
        Player.BuffStatCompo.AddBuff(buffSO, 2, (int)CombineLevel);

        CombatMarkingData forgeData =
        new CombatMarkingData(BuffingType.Smelting,
        "진정한 강철은 수만번의 담금질 속에서 만들어진다.",
        int.MaxValue);

        BattleReader.CombatMarkManagement.AddBuffingData(Player, CardID, forgeData, buffSO.stackBuffs[0].values[(int)CombineLevel]);

        Debug.Log($"Current Forging Stat: {Player.BuffStatCompo.GetStack(StackEnum.Forging)}");
    }
}
