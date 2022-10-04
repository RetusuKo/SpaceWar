using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 PlayerPosition;
    public SerializableDictonary<string, bool> PlayerUpgrade;

    public GameData()
    {
        PlayerUpgrade = new SerializableDictonary<string, bool>(); 
    }
}
