using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTVisual;

public class EarthquakeAttack : SampleBossNode
{
    private readonly int _earthquakeAnimHash = Animator.StringToHash("EarthQuake");

    [SerializeField] private int _quakeCnt = 3;
    private int _remainCnt;

    protected override void OnAnimationEndHandle()
    {

    }

    protected override void OnAnimationEventHandle()
    {
        PoolManager.Instance.Pop(attackParticle).transform.position = brain.transform.position + Vector3.down * 1.5f;

        GameObject obj = Instantiate(hitParticle.gameObject);
        obj.transform.position = brain.target.transform.position;
        Destroy(obj, 1.0f);

        SoundManager.PlayAudioRandPitch(attackSound);

        brain.target.HealthCompo.ApplyDamage(Mathf.RoundToInt(brain.CharStat.GetDamage() * 0.2f), brain);
        FeedbackManager.Instance.ShakeScreen(2f);
        if (--_remainCnt > 0) brain.AnimatorCompo.SetTrigger(_earthquakeAnimHash);

    }

    protected override void OnStart()
    {
        base.OnStart();

        _remainCnt = _quakeCnt;
        brain.AnimatorCompo.SetTrigger(_earthquakeAnimHash);
    }


    protected override void OnStop()
    {
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
