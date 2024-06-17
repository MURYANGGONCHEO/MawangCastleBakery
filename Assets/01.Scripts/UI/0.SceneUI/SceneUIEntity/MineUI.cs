using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineUI : SceneUI
{
    public MineInfo CurrentStage { get; set; }

    [SerializeField]
    protected GameObject _tutorialPanel;

    public override void SceneUIStart()
    {
        base.SceneUIStart();

        SceneObserver.BeforeSceneType = SceneType.Lobby;

        CheckOnFirst cf = DataManager.Instance.LoadData<CheckOnFirst>(DataKeyList.checkIsFirstPlayGameDataKey);
        if (!cf.isFirstOnEnterDungeon)
        {
            _tutorialPanel.SetActive(true);
            cf.isFirstOnEnterDungeon = true;
            DataManager.Instance.SaveData(cf, DataKeyList.checkIsFirstPlayGameDataKey);
        }
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
