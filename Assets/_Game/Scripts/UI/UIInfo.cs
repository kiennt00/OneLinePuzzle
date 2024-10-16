using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInfo : UICanvas
{
    [SerializeField] Button btnTutorial;
    [SerializeField] TextMeshProUGUI textGold, textLevel;

    private void Awake()
    {
        btnTutorial.onClick.AddListener(() =>
        {
            UIManager.Ins.OpenUI<UITutorial>();
        });

        this.RegisterListener(EventID.OnGoldChanged, (param) =>
        {
            StartCoroutine(IEOnGoldChanged((int)param));
        });

        this.RegisterListener(EventID.OnLevelChanged, (param) =>
        {
            UpdateTextLevel(DataManager.Ins.GetCurrentLevel());
        });
        
        UpdateTextLevel(DataManager.Ins.GetCurrentLevel());
    }

    private void UpdateTextGold(int gold)
    {
        textGold.text = gold.ToString();
    }

    private void UpdateTextLevel(int level)
    {
        textLevel.text = "Level " + (level + 1).ToString();
    }

    IEnumerator IEOnGoldChanged(int gold)
    {
        int currentGold = DataManager.Ins.GetCurrentGold();
        for (int i = 0; i < 9; i++)
        {
            UpdateTextGold(currentGold - gold / 9 * (8 - i));
            yield return Constants.WFS_0_S_1;
        }
    }

    private void OnEnable()
    {
        UpdateTextGold(DataManager.Ins.GetCurrentGold());
    }
}
