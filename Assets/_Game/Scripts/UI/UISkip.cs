using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkip : UICanvas
{
    [SerializeField] Button btnSkip, btnQuit;
    [SerializeField] TextMeshProUGUI textCountdown, textPrice;
    [SerializeField] Transform ImageCountdown;
    [SerializeField] int price = 500, initialCountdown = 30;
    int countdown;

    private void Awake()
    {
        btnSkip.onClick.AddListener(() =>
        {
            if (DataManager.Ins.GetCurrentGold() >= price)
            {
                CloseDirectly();
                DataManager.Ins.AdjustGold(-price);
                LevelManager.Ins.NextLevel();                
            }
        });

        btnQuit.onClick.AddListener(() =>
        {
            CloseDirectly();
            UIManager.Ins.OpenUI<UIGameplay>();
        });

        textPrice.text = price.ToString();
    }

    private void Update()
    {
        ImageCountdown.Rotate(0f, 0f, -360f * Time.deltaTime);
    }

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeGameState(GameState.Skip);
        countdown = initialCountdown;
        Countdown();
    }

    IEnumerator IECountdown()
    {
        yield return Constants.WFS_1_S;
        Countdown();
    }

    void Countdown()
    {
        if (countdown < 0)
        {
            CloseDirectly();
            LevelManager.Ins.NextLevel();
        }
        else
        {
            AudioManager.Ins.PlaySFX(SFXType.Countdown);
            textCountdown.text = countdown.ToString();
            countdown--;
            StartCoroutine(IECountdown());
        }
    }
}
