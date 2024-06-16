using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleContent : Content
{
    public CutScene cutScene;
    [SerializeField] private Collider2D _contentConfiner;
    public Collider2D ContentConfiner => _contentConfiner;

    public override void ContentStart()
    {
        Camera.main.orthographic = false;
        if (StageManager.Instanace.SelectStageData.stageCutScene is not null)
            cutScene = Instantiate(StageManager.Instanace.SelectStageData.stageCutScene, transform);
    }
    public override void ContentEnd()
    {
        
    }
    public void OnDestroy()
    {
        Camera.main.orthographic = true;
    }
}
