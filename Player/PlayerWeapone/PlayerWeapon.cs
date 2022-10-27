using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeapon : Weapon
{
    public PlayerInfo.WeaponType CurentWeapon;

    protected int _currentAttack = 0;
    protected void Initalize(float weaponSpeed, PlayerInfo.WeaponType weaponTipe)
    {
        _weaponSpeed = weaponSpeed;
        CurentWeapon = weaponTipe;
        GetAllComponent();
    }
    public void AtackConstruct(string animName, bool airAttack = false)
    {
        if (!_isWeaponCollDown)
        {
            _currentAttack++;
            if (_currentAttack > 3)
                _currentAttack = 1;
            if (PlayerInfo.TimeSinceAttack > 1.0f)
                _currentAttack = 1;
            if(airAttack)
                _currentAttack = 3;
            _animator.SetTrigger("Attack" + _currentAttack);
            PlayerInfo.TimeSinceAttack = 0.0f;
            WeaponActivate();
        }
    }
    protected abstract void WeaponActivate();
}