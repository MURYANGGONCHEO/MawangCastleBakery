using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerDead : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;

    public void DeadSeq()
    {
        UIManager.Instance.GetSceneUI<BattleUI>().SetResult(false);
    }
}
