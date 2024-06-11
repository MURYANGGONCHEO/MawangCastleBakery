using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleContent : Content
{
    public CompositeCollider2D contentConfiner;
    public CutScene cutScene;
    public override void ContentStart()
    {
        Camera.main.orthographic = false;
        if (MapManager.Instanace.SelectStageData.stageCutScene is not null)
            cutScene = Instantiate(MapManager.Instanace.SelectStageData.stageCutScene, transform);
    }
    public override void ContentEnd()
    {
        Camera.main.orthographic = true;
    }
}
