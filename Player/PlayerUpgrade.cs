using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour, IDatePersistance
{
    private static Dictionary<string, bool> _upgrades = new Dictionary<string, bool>();
    public static string[] UpgradesName = new string[] { "TeleportingUpgrade", "JumpDownUpgrade", "HaveGun", "WallJumpUpgrade", "GunExplode" };
    public static void UpgradePut()
    {
        for (int i = 0; i < UpgradesName.Length; i++)
            _upgrades.Add(UpgradesName[i], false);
    }
    public static void UpgradeChangeBool(string upgradeName, bool upgrade = true)
    {
        _upgrades[upgradeName] = upgrade;
    }
    public static bool UpgradeCheck(string upgradeName)
    {
        _upgrades.TryGetValue(upgradeName, out bool returnValue);
        return returnValue;
    }
    public void LoadDate(GameData data)
    {
        if (data.PlayerUpgrade.Count > 0)
        {
            for (int i = 0; i < _upgrades.Count; i++)
                if (data.PlayerUpgrade[UpgradesName[i]])
                    _upgrades[UpgradesName[i]] = true;
        }
    }
    public void SaveData(GameData data)
    {
        data.PlayerUpgrade.Clear();
        for (int i = 0; i < _upgrades.Count; i++)
            data.PlayerUpgrade.Add(UpgradesName[i], _upgrades[UpgradesName[i]]);
    }
}
