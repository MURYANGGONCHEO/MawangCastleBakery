using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[Serializable]
public struct TutorialPanel
{
    public Sprite img;
    public string desc;
}

public abstract class TutorialBase : MonoBehaviour
{
    [SerializeField]
    protected List<TutorialPanel> _panelList;

    [SerializeField]
    protected Image _curPanelImg;
    [SerializeField]
    protected TextMeshProUGUI _curPanelDesc;

    protected int _panelIdx = 0;

    public void FlipPageNext() => _panelIdx = Mathf.Clamp(_panelIdx + 1, 0, _panelList.Count - 1);
    public void FlipPageBefore() => _panelIdx = Mathf.Clamp(_panelIdx - 1, 0, _panelList.Count - 1);

    public void UpdatePanel()
    {
        _curPanelImg.sprite = _panelList[_panelIdx % _panelList.Count].img;
        _curPanelDesc.text = _panelList[_panelIdx % _panelList.Count].desc;
    }
}