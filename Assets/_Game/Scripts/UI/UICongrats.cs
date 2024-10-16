using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICongrats : UICanvas
{
    [SerializeField] Button btnPlay, btnHome;

    private void Awake()
    {
        btnPlay.onClick.AddListener(() =>
        {
            DataManager.Ins.SaveCurrentLevel(0);
            CloseDirectly();
            LevelManager.Ins.LoadLevel(DataManager.Ins.GetCurrentLevel());
        });

        btnHome.onClick.AddListener(() =>
        {
            CloseDirectly();
            UIManager.Ins.OpenUI<UIMainmenu>();
        });
    }

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeGameState(GameState.Congrats);
    }
}
