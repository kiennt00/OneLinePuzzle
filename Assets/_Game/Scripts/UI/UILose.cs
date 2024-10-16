using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILose : UICanvas
{
    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeGameState(GameState.Lose);
        //AudioManager.Ins.PlaySFX(SFXType.Lose);
        //screen shake?
    }
}
