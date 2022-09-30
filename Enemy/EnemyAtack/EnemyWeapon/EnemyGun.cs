using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : EnemyWeapon
{
    [SerializeField] private EnemyAtack _enemyAtack;
    [SerializeField] private GameObject _bulletPrefab;
    private void Awake()
    {
        Initalize(weaponSpeed: 0.2f);
    }
    protected override void WeaponActivate()
    {
        _enemyAtack.ActivateGunEnemyAttack(_spriteRenderer, _bulletPrefab);
        _enemyAtack.ActivateWeapone(true);
        StartCoroutine(TimeActvateWeapone(0.2f, enemyAtack:_enemyAtack));
        _isWeaponCollDown = true;
        StartCoroutine(WeaponCoolDown(_weaponSpeed));
    }
    public void Atack()
    {
        WeaponActivate();
    }
}
