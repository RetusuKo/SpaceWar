using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 PlayerPosition = new Vector3(0,0,0);
    public SerializableDictonary<string, bool> PlayerUpgrade;

    public GameData()
    {
        PlayerUpgrade = new SerializableDictonary<string, bool>(); 
    }
}
