using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Particle;

namespace BTVisual
{
    public abstract class SampleBossNode : ActionNode
    {
        [SerializeField] protected AudioClip attackSound;
        [SerializeField] protected ParticleSystem hitParticle;                           
        [SerializeField] protected PoolingType attackParticle;

        [SerializeField] protected string parametorName;
        protected int animationHash;


        protected virtual void OnEnable()
        {
            animationHash = Animator.StringToHash(parametorName);
        }

        protected override void OnStart()
        {
            brain.AnimatorCompo.SetBool(animationHash, true);
            brain.BossAnimator.OnAnimationEvent += OnAnimationEventHandle;
            brain.BossAnimator.OnAnimationEnd += OnAnimationEndHandle;
        }

        protected override void OnStop()
        {
            brain.AnimatorCompo.SetBool(animationHash, false);
            brain.BossAnimator.OnAnimationEvent -= OnAnimationEventHandle;
            brain.BossAnimator.OnAnimationEnd -= OnAnimationEndHandle;
        }

        protected abstract void OnAnimationEventHandle();
        protected abstract void OnAnimationEndHandle();
    }
}