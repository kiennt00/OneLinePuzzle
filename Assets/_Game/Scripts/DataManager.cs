using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public bool IsFirstAttempt()
    {
        if (!PlayerPrefs.HasKey(Constants.PP_NOT_FIRST_ATTEMPT))
        {
            PlayerPrefs.SetInt(Constants.PP_NOT_FIRST_ATTEMPT, 1);
            return true;
        }

        return false;
    }

    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(Constants.PP_CURRENT_LEVEL, 0);
    }

    public void SaveCurrentLevel(int levelIndex)
    {
        PlayerPrefs.SetInt(Constants.PP_CURRENT_LEVEL, levelIndex);

        this.PostEvent(EventID.OnLevelChanged);
    }

    public void SaveCurrentGold(int gold)
    {
        PlayerPrefs.SetInt(Constants.PP_CURRENT_GOLD, gold);        
    }

    public int GetCurrentGold()
    {
        return PlayerPrefs.GetInt(Constants.PP_CURRENT_GOLD, 0);
    }

    public void AdjustGold(int gold)
    {
        int currentGold = GetCurrentGold();
        currentGold += gold;
        SaveCurrentGold(currentGold);
        this.PostEvent(EventID.OnGoldChanged, gold);
    }
}
