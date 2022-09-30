using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    private void Awake()
    {
        UpgradePut();
    }
    private static Dictionary<string, bool> Upgrades = new Dictionary<string, bool>();
    public static void UpgradePut()
    {
        Upgrades.Add("TeleportingUpgrade", false);
        Upgrades.Add("JumpDownUpgrade", false);
        Upgrades.Add("HaveGun", false);
        Upgrades.Add("WallJumpUpgrade", false);
        Upgrades.Add("GunExplode", false);
    }
    public static void UpgradeChangeBool(string upgradeName, bool upgrade = true)
    {
        Upgrades[upgradeName] = upgrade;
    }
    public static bool UpgradeCheck(string upgradeName)
    {
        Upgrades.TryGetValue(upgradeName, out bool returnValue);
        return returnValue;
    }
}
