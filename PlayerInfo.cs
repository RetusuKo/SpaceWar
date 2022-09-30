using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static bool[] Upgrades = new bool[] 
    { 
        false, //Summary: TeleportingUpgrade
        false, //Summary: JumpDownUpgrade
        false, //Summary: HaveGun
        false, //Summary: WallJumpUpgrade
        false  //Summary: GunExplode
    };
    public static string[] UpgradesName = new string[]
    {
        "TeleportingUpgrade",
        "JumpDownUpgrade", 
        "HaveGun",
        "WallJumpUpgrade",
        "GunExplode"
    };
    public static bool TeleportingUpgrade = false;
    public static bool JumpDownUpgrade = false;
    public static bool HaveGun = false;
    public static bool WallJumpUpgrade = false;
    public static bool GunExplode = false;

    public static bool IsAttacking = false;
    public static bool WatchRight = false;
    public static bool DoNotTakeDamage = false;
    public static float TimeSinceAttack = 0f;
    public static WeaponType CurentWapone;

    public static float NoDomageMiliSec = 0.5f;
    public enum WeaponType
    {
        Sword,
        Gun
    }
    public void SetUpgradeValue()
    {
        for (int i = 0; i < UpgradesName.Length; i++)
            PlayerPrefs.SetInt(UpgradesName[i], (Upgrades[i] ? 1 : 0));
    }
    public void GetUpgradeValue()
    {
        for (int i = 0; i < Upgrades.Length; i++)
            Upgrades[i] = (PlayerPrefs.GetInt(UpgradesName[i]) != 0);
    }
}