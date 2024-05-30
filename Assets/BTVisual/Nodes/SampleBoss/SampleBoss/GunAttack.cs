using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttack : SampleBossNode
{
    private readonly int _fireHash = Animator.StringToHash("Fire");

    [SerializeField] private int _attackCnt;
    private int _remainCnt;

    protected override void OnAnimationEndHandle()
    {
    }

    protected override void OnAnimationEventHandle()
    {
        GameObject obj = Instantiate(hitParticle.gameObject);
        obj.transform.position = brain.target.transform.position;
        Destroy(obj, 1.0f);
        SoundManager.PlayAudioRandPitch(attackSound);
        brain.target.HealthCompo.ApplyDamage(Mathf.RoundToInt(brain.CharStat.GetDamage() * 0.3f), brain);

        FeedbackManager.Instance.ShakeScreen(1.5f);
        if (--_remainCnt > 0) brain.AnimatorCompo.SetTrigger(_fireHash);
    }

    protected override void OnStart()
    {
        Debug.Log("ÃÑ ½ÃÀÛ");
        base.OnStart();
        _remainCnt = _attackCnt;
        brain.AnimatorCompo.SetTrigger(_fireHash);

    }

    protected override void OnStop()
    {
        Debug.Log("ÃÑ³¡");
        _remainCnt = 0;
        base.OnStop();
    }

    protected override State OnUpdate()
    {
        if (_remainCnt > 0)
        {
            return State.RUNNING;
        }
        return State.SUCCESS;
    }
}
