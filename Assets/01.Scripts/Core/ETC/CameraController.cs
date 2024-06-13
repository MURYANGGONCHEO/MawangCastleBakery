using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _vCam;
    private PoolVCam _poolVCam;
    private Transform _target;

    public PoolVCam CaomObj { get; private set; }
    public BattleController BattleController { get; set; }
    private Dictionary<CameraTargetType, Action<Vector2, float, float, float, float, Ease>> _targetActionDic = new();
    private bool _camOnMoving = false;

    private Sequence _toPlayerSeq;
    private Sequence _toEnemySeq;

    private void Awake()
    {
        _target = GameManager.Instance.gameObject.transform.Find("CameraTrm");
        _vCam = UIManager.Instance.VirtualCamera;
    }

    private void Start()
    {
        _targetActionDic.Add(CameraTargetType.Player, HandleCamraTargettingPlayer);
        _targetActionDic.Add(CameraTargetType.Enemy, HandleCamraTargettingEmeny);
    }

    private void SequenceClear()
    {
        _toPlayerSeq.Kill();
        _toEnemySeq.Kill();

        _toPlayerSeq = DOTween.Sequence();
        _toEnemySeq = DOTween.Sequence();
    }

    private void HandleCamraTargettingPlayer(Vector2 mValue, float rValue, float zValue, float duration, float delayTime, Ease easing)
    {
        _toPlayerSeq.Append(_target.DOLocalMove(mValue, duration).SetEase(easing));
        _toPlayerSeq.Join(_poolVCam.transform.DORotate(new Vector3(0, 0, rValue), duration).SetEase(easing));
        _toPlayerSeq.Join(DOTween.To(() => _vCam.m_Lens.FieldOfView, o => _vCam.m_Lens.FieldOfView = o, 60 + zValue, duration).SetEase(easing));
        _toPlayerSeq.AppendInterval(delayTime);
        _toPlayerSeq.OnComplete(() => _camOnMoving = true);
    }

    private void HandleCamraTargettingEmeny(Vector2 mValue, float rValue, float zValue, float duration, float delayTime, Ease easing)
    {
        _toEnemySeq.Append(_target.DOLocalMove(mValue, duration).SetEase(easing));
        _toEnemySeq.Join(_poolVCam.transform.DORotate(new Vector3(0, 0, rValue), duration).SetEase(easing));
        _toEnemySeq.Join(DOTween.To(() => _vCam.m_Lens.FieldOfView, o => _vCam.m_Lens.FieldOfView = o, 60 + zValue, duration).SetEase(easing));
        _toEnemySeq.AppendInterval(delayTime);
        _toEnemySeq.OnComplete(() => _camOnMoving = true);
    }

    public void SetTransitionTime(float time)
    {
        UIManager.Instance.CinemachineBrain.m_DefaultBlend.m_Time = time;
    }

    public void StartCameraSequnce(CameraMoveTypeSO moveType, Action endCallBack = null)
    {
        _poolVCam = PoolManager.Instance.Pop(PoolingType.VCamPool) as PoolVCam;
        _vCam = _poolVCam.VCam;
        _vCam.Follow = _target;

        StartCoroutine(CameraSequenceCo(moveType.camMoveSequenceList, endCallBack));
    }

    private IEnumerator CameraSequenceCo(List<CameraMoveSequence> sequenceList, Action endCallBack = null)
    {
        foreach (CameraMoveSequence seq in sequenceList)
        {
            SetTransitionTime(seq.cameraTransitionTime);
            _camOnMoving = false;
            SequenceClear();

            _targetActionDic[seq.cameraTarget].
            Invoke(seq.movingValue * (int)seq.cameraTarget,
                   seq.rotationValue * (int)seq.cameraTarget,
                   seq.zoonInValue,
                   seq.duration,
                   seq.delayTime,seq.easingType);

            if (seq.shakeDefination.isShaking)
            {
                FeedbackManager.Instance.ShakeScreen(Vector2.one * 0.2f,
                                                     seq.shakeDefination.seconds);
            }

            yield return new WaitUntil(() => _camOnMoving);
        }
        PoolManager.Instance.Push(_poolVCam);
        endCallBack?.Invoke();
    }
}
