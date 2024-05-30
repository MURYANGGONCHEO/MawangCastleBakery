using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleContent : Content
{
    public CompositeCollider2D contentConfiner;
    public CutScene cutScene;
    public override void ContentStart()
    {
        if (MapManager.Instanace.SelectStageData.stageCutScene is not null)
            cutScene = Instantiate(MapManager.Instanace.SelectStageData.stageCutScene, transform);
    }
}
