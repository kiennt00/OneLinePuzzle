using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    public List<Level> listLevel = new();
    public Level currentLevel;
    public int currentLevelIndex;

    public Material green, red;
    public List<ParticleSystem> listParticle = new();

    public void LoadLevel(int levelIndex)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        currentLevelIndex = levelIndex;

        if (currentLevelIndex >= listLevel.Count)
        {
            UIManager.Ins.CloseUI<UIInfo>();
            UIManager.Ins.OpenUI<UICongrats>();
            return;
        }

        currentLevel = Instantiate(listLevel[levelIndex]);
        UIManager.Ins.OpenUI<UIInfo>();
        UIManager.Ins.OpenUI<UIGameplay>();
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        DataManager.Ins.SaveCurrentLevel(currentLevelIndex);
        LoadLevel(currentLevelIndex);
    }

    public void CheckFinish()
    {
        if (currentLevel.CheckFinish())
        {
            UIManager.Ins.CloseUI<UIGameplay>();
            UIManager.Ins.OpenUI<UIWin>();
            int randomIndex = Random.Range(0, listParticle.Count);
            listParticle[randomIndex].Play();
            DataManager.Ins.AdjustGold(100);
        }
        else
        {
            currentLevel.Lose();
        }
    }
}
