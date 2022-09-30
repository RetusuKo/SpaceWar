using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
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
}