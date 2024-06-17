using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineUI : SceneUI
{
    public MineInfo CurrentStage { get; set; }

    public override void SceneUIStart()
    {
        base.SceneUIStart();

        SceneObserver.BeforeSceneType = SceneType.Lobby;
    }

    public void GotoEditDeck()
    {
        GameManager.Instance.ChangeScene(SceneType.deckBuild);
    }

    public void GoToBattle()
    {
        GameManager.Instance.ChangeScene(SceneType.battle);
    }
}
