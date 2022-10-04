using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDatePersistance
{
    void LoadDate(GameData data);

    void SaveData(GameData data);
}
