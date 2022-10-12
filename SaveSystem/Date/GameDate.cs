using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public long LastUpdated;
    public Vector3 PlayerPosition = new Vector3(0,0,0);
    public SerializableDictonary<string, bool> PlayerUpgrade;
    public string SaveSceneName;

    public GameData()
    {
        PlayerUpgrade = new SerializableDictonary<string, bool>();
        SaveSceneName = "Lvl 1";
    }
    public int GetPercentageComplate()
    {
        int totalLvl = SceneManager.sceneCount - 1;
        return totalLvl * 100 / SceneManager.GetActiveScene().rootCount;
    }
}
