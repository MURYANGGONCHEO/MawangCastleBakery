using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : BossBrain
{
    private readonly int jumpHash = Animator.StringToHash("Jump");
    [SerializeField] private BuffSO pattenBuff;
    [SerializeField] private AudioClip _landingSound;
    protected override void OnEnable()
    {
        base.OnEnable();
        BuffStatCompo.AddBuff(pattenBuff, 5);
    }
    protected override void OnDisable()
    {

        base.OnDisable();
    }
    public override void SlowEntityBy(float percent)
    {

    }

    public override void Spawn(Vector3 spawnPos, Action callBack = null)
    {
        SpriteRendererCompo.material.SetFloat("_dissolve_amount", 0);

        AnimatorCompo.SetBool(spawnAnimationHash, true);

        transform.position = spawnPos + new Vector3(-4f, 20f);
        transform.DOMoveX(spawnPos.x, 0.5f);
        transform.DOMoveY(spawnPos.y, 0.5f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            PoolManager.Instance.Pop(PoolingType.GroundCrack).transform.position = transform.position + Vector3.down * 1.5f;
            SoundManager.PlayAudio(_landingSound, true);
            FeedbackManager.Instance.ShakeScreen(4f);

            AnimatorCompo.SetBool(spawnAnimationHash, false);
            callBack?.Invoke();
            turnStatus = TurnStatus.Ready;
        });
    }
    protected override void HandleEndMoveToOriginPos()
    {
        SpriteRendererCompo.flipX = !SpriteRendererCompo.flipX;
        AnimatorCompo.SetBool(jumpHash, false);

    }

    protected override void HandleEndMoveToTarget()
    {
        AnimatorCompo.SetBool(jumpHash, false);

    }

    public override void MoveToTargetForward(Vector3 pos)
    {
        base.MoveToTargetForward(pos);
        AnimatorCompo.SetBool(jumpHash, true);
    }

    public override void MoveToOriginPos()
    {
        base.MoveToOriginPos();
        SpriteRendererCompo.flipX = !SpriteRendererCompo.flipX;
        AnimatorCompo.SetBool(jumpHash, true);
    }
}
