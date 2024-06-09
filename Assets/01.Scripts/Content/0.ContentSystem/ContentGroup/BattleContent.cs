using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleContent : Content
{
    public CompositeCollider2D contentConfiner;

    public override void ContentStart()
    {
        Camera.main.orthographic = false;
    }

    public override void ContentEnd()
    {
        Camera.main.orthographic = true;
    }
}
