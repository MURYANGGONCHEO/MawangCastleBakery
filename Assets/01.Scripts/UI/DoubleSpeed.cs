using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleSpeed : MonoBehaviour
{
    [SerializeField] private Color[] _colors;

    private bool _isDoubleSpeed = false;
    private Image _img;

    private void Awake()
    {
        _img = GetComponent<Image>();
    }

    public void DoubleSpeedAction()
    {
        _isDoubleSpeed = !_isDoubleSpeed;

        if(_isDoubleSpeed )
        {
            _img.color = _colors[1];
            Time.timeScale = 2;
        }
        else
        {
            _img.color = _colors[0];
            Time.timeScale = 1;
        }
    }
}
