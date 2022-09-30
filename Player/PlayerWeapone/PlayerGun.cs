using UnityEngine;

public class PlayerGun : PlayerWeapon
{
    [SerializeField] private PlayerAtack _playerShoot;
    [SerializeField] private GameObject _gunBullets;
    [SerializeField] private ParticleSystem _explo;
    //private PlayerBullet _bullet;
    private void Awake()
    {
        Initalize(weaponSpeed: 0.2f, weaponTipe: PlayerInfo.WeaponType.Gun);
        //_bullet = _gunBullets.GetComponent<PlayerBullet>();
    }
    protected override void WeaponActivate()
    {
        _playerShoot.ActivateGunAttack(_spriteRenderer, _gunBullets);
        _playerShoot.ActivateWeapone(true);
        StartCoroutine(TimeActvateWeapone(0.2f, _playerShoot));
        _isWeaponCollDown = true;
        StartCoroutine(WeaponCoolDown(_weaponSpeed, playerIsAttacking: true));
    }
    public void Atack()
    {
        AtackConstruct("Attack");
    }
    private void ExploteedShoot()
    { 

    }
}