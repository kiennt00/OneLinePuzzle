using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : UICanvas
{
    [SerializeField] Button btnNext;

    private void Awake()
    {
        btnNext.onClick.AddListener(() =>
        {
            CloseDirectly();
            LevelManager.Ins.NextLevel();
        });
    }

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeGameState(GameState.Win);
    }
}
