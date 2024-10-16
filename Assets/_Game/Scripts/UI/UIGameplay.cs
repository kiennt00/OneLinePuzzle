using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : UICanvas
{
    [SerializeField] Button btnSkip;

    private void Awake()
    {
        btnSkip.onClick.AddListener(() =>
        {
            CloseDirectly();
            UIManager.Ins.OpenUI<UISkip>();
        });
    }

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeGameState(GameState.Gameplay);
    }
}
