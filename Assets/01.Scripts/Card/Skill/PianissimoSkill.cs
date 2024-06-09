using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianissimoSkill : MusicCardBase, ISkillEffectAnim
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

        bool TargetIsOne = Player.GetSkillTargetEnemyList[this].Count == 1 ? true : false;

        if(TargetIsOne)
        {
            Entity e = Player.GetSkillTargetEnemyList[this][0];
            Player.VFXManager.PlayParticle(this, e.transform.position);
            for(int i = 0; i < 2; ++i)
            {
                e?.HealthCompo.ApplyDamage(GetDamage(CombineLevel), Player);
                if(e != null)
                {
                    GameObject obj = Instantiate(CardInfo.hitEffect.gameObject, e.transform.position, Quaternion.identity);
                    Destroy(obj, 1.0f);
                }
                yield return new WaitForSeconds(0.4f);
            }
        }
        else
        {
            Player.VFXManager.PlayParticle(this);
            foreach (var e in Player.GetSkillTargetEnemyList[this])
            {
                e?.HealthCompo.ApplyDamage(GetDamage(CombineLevel), Player);
                if (e != null)
                {
                    GameObject obj = Instantiate(CardInfo.hitEffect.gameObject, e.transform.position, Quaternion.identity);
                    Destroy(obj, 1.0f);
                }
                yield return new WaitForSeconds(0.4f);
            }
        }

        Player.BuffStatCompo.AddStack(StackEnum.DEFMusicalNote, buffSO.stackBuffs[0].values[(int)CombineLevel]);
        Debug.Log($"Stacks: DEF({Player.BuffStatCompo.GetStack(StackEnum.DEFMusicalNote)}) / DMG({Player.BuffStatCompo.GetStack(StackEnum.DMGMusicaldNote)}) / FAINT({Player.BuffStatCompo.GetStack(StackEnum.FAINTMusicalNote)})");
    }
}
