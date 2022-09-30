using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWeapon : Weapon
{
    protected void Initalize(float weaponSpeed)
    {
        _weaponSpeed = weaponSpeed;
        GetAllComponent();
    }
    protected abstract void WeaponActivate();
}
