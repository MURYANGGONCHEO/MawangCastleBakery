using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncoreSkill : MusicCardBase, ISkillEffectAnim
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
        Player.VFXManager.PlayParticle(CardInfo, Player.transform.position, (int)CombineLevel, _skillDurations[(int)CombineLevel]);
        SoundManager.PlayAudio(_soundEffect, false);
        StartCoroutine(AttackCor());
        Player.OnAnimationCall -= HandleAnimationCall;
    }

    public void HandleEffectEnd()
    {
        Player.EndAbility();
        Player.VFXManager.EndParticle(CardInfo, (int)CombineLevel);
        IsActivingAbillity = false;
        Player.VFXManager.OnEndEffectEvent -= HandleEffectEnd;
    }

    private IEnumerator AttackCor()
    {
        yield return new WaitForSeconds(0.3f);

        Player.BuffStatCompo.AddStack(StackEnum.DEFMusicalNote, buffSO.stackBuffs[0].values[(int)CombineLevel]);
        Player.BuffStatCompo.AddStack(StackEnum.DMGMusicaldNote, buffSO.stackBuffs[0].values[(int)CombineLevel]);
        Player.BuffStatCompo.AddStack(StackEnum.FAINTMusicalNote, buffSO.stackBuffs[0].values[(int)CombineLevel]);
        Debug.Log($"Stacks: DEF({Player.BuffStatCompo.GetStack(StackEnum.DEFMusicalNote)}) / DMG({Player.BuffStatCompo.GetStack(StackEnum.DMGMusicaldNote)}) / FAINT({Player.BuffStatCompo.GetStack(StackEnum.FAINTMusicalNote)})");
    }
}
