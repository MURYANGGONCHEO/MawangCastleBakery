using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public abstract class TutorialBase : MonoBehaviour
{
    [SerializeField]
    protected VideoPlayer _videoPlayer;
    [SerializeField]
    protected string _videoPath;

    public TutorialBase()
    {
        _videoPlayer.url = _videoPath;
    }
}
