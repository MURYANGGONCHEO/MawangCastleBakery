using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _vCam;

    public PoolVCam CaomObj { get; private set; }
    public BattleController BattleController { get; set; }

    private Dictionary<CameraTargetType, Action<float, float, Ease>> _targetActionDic = new ();

    private bool _camOnMoving = false;

    private void Awake()
    {
        _vCam = UIManager.Instance.VirtualCamera;
    }

    private void Start()
    {
        _targetActionDic.Add(CameraTargetType.Player, HandleCamraTargettingPlayer);
        _targetActionDic.Add(CameraTargetType.Enemy, HandleCamraTargettingEmeny);
    }

    private void HandleCamraTargettingPlayer(float value, float duration, Ease easing)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_vCam.transform.DOLocalMoveX(value, duration).SetEase(easing));
        seq.Join(_vCam.transform.DORotate(new Vector3(0, 0, 0.5f), duration).SetEase(easing));
        seq.Join(DOTween.To(() => 5, o => _vCam.m_Lens.OrthographicSize = o, 5, duration).SetEase(easing));
        seq.OnComplete(() => _camOnMoving = true);
    }

    private void HandleCamraTargettingEmeny(float value, float duration, Ease easing)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_vCam.transform.DOLocalMoveX(value, duration).SetEase(easing));
        seq.Join(_vCam.transform.DORotate(new Vector3(0, 0, -0.5f), duration).SetEase(easing));
        seq.Join(DOTween.To(() => 5, o => _vCam.m_Lens.OrthographicSize = o, 5, duration).SetEase(easing));
        seq.OnComplete(() => _camOnMoving = true);
    }

    public void SetTransitionTime(float time)
    {
        UIManager.Instance.CinemachineBrain.m_DefaultBlend.m_Time = time;
    }

    public void StartCameraSequnce(CameraMoveTypeSO moveType)
    {
        StartCoroutine(CameraSequenceCo(moveType.camMoveSequenceList));
    }

    private IEnumerator CameraSequenceCo(List<CameraMoveSequence> sequenceList)
    {
        foreach(CameraMoveSequence seq in sequenceList)
        {
            SetTransitionTime(seq.cameraTransitionTime);

            _camOnMoving = false;
            _targetActionDic[seq.cameraTarget].Invoke(seq.movingValue * (int)seq.cameraTarget, seq.duration, seq.easingType);
            Debug.Log(1);
            yield return new WaitUntil(() => _camOnMoving);
        }

        SetDefaultCam();
    }
    
    public void SetDefaultCam()
    {
        SetTransitionTime(1);

        Sequence seq = DOTween.Sequence();
        seq.Append(_vCam.transform.DOLocalMoveX(0, 0.3f).SetEase(Ease.Linear));
        seq.Join(_vCam.transform.DORotate(new Vector3(0, 0, 0), 0.3f).SetEase(Ease.Linear));
        seq.Join(DOTween.To(() => 5, o => _vCam.m_Lens.OrthographicSize = o, 6, 0.3f).SetEase(Ease.Linear));
    }
}
