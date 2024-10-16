using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial : UICanvas
{
    [SerializeField] Button btnOK;

    private void Awake()
    {
        btnOK.onClick.AddListener(() =>
        {
            CloseDirectly();
            GameManager.Ins.ChangeGameState(GameState.Gameplay);
        });
    }

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeGameState(GameState.Tutorial);
    }
}
