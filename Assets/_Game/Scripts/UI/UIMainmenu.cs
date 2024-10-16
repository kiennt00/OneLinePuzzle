using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainmenu : UICanvas
{
    [SerializeField] Button btnPlay;

    private void Awake()
    {
        btnPlay.onClick.AddListener(() =>
        {
            CloseDirectly();
            LevelManager.Ins.LoadLevel(DataManager.Ins.GetCurrentLevel());
            if (DataManager.Ins.IsFirstAttempt())
            {
                UIManager.Ins.OpenUI<UITutorial>();
            }
        });
    }

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeGameState(GameState.Mainmenu);
    }
}
