using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : SampleBossNode
{
    private bool isAttacking;

    protected override void OnAnimationEndHandle()
    {
        brain.MoveToOriginPos();
    }
    protected override void OnAnimationEventHandle()
    {
        GameObject obj = Instantiate(hitParticle.gameObject);
        obj.transform.position = brain.target.transform.position;
        Destroy(obj, 1.0f);
        SoundManager.PlayAudioRandPitch(attackSound, true);
        brain.target.HealthCompo.ApplyDamage(brain.CharStat.GetDamage() * 2, brain);
        FeedbackManager.Instance.ShakeScreen(4f);
    }

    protected override void OnStart()
    {
        brain.OnMoveTarget += MoveTargetPosHandle;
        brain.OnMoveOriginPos += MoveOriginPosHandle;
        brain.MoveToTargetForward(brain.target.forwardTrm.position);

        isAttacking = true;
    }
    private void MoveTargetPosHandle()
    {
        brain.VFXPlayer.PlayParticle(parametorName);

        brain.AnimatorCompo.SetBool(animationHash, true);
        brain.BossAnimator.OnAnimationEvent += OnAnimationEventHandle;
        brain.BossAnimator.OnAnimationEnd += OnAnimationEndHandle;
    }
    private void MoveOriginPosHandle()
    {
        isAttacking = false;
    }
    protected override void OnStop()
    {
        brain.OnMoveTarget -= MoveTargetPosHandle;
        brain.OnMoveOriginPos -= MoveOriginPosHandle;
        base.OnStop();
    }

    protected override State OnUpdate()
    {
        if (isAttacking)
        {
            return State.RUNNING;
        }
        return State.SUCCESS;
    }
}
