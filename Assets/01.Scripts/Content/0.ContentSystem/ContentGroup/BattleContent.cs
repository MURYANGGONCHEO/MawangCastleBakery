using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleContent : Content
{
    [SerializeField] private Collider2D _contentConfiner;
    public Collider2D ContentConfiner => _contentConfiner;

    public override void ContentStart()
    {
        Camera.main.orthographic = false;
    }

    public override void ContentEnd()
    {
        Camera.main.orthographic = true;
    }
}
