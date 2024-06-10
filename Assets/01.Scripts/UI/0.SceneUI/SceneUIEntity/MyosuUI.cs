using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyosuUI : SceneUI
{
    [SerializeField] private CrossRoadsAppearText _appearText;

    [SerializeField]
    private GameObject _tutorialPanel;

    public override void SceneUIStart()
    {
        base.SceneUIStart();

        CheckOnFirst cf = DataManager.Instance.LoadData<CheckOnFirst>(DataKeyList.checkIsFirstPlayGameDataKey);
        if (!cf.isFirstOnEnterMaze)
        {
            _tutorialPanel.SetActive(true);
            cf.isFirstOnEnterMaze = true;
            DataManager.Instance.SaveData(cf, DataKeyList.checkIsFirstPlayGameDataKey);
        }
    }

    public void ApearText()
    {
        _appearText.Show();
    }

    public void HideText()
    {
        _appearText.Hide();
    }

    public void GoToBattleScene()
    {
        GameManager.Instance.ChangeScene(SceneType.battle);
    }
}
