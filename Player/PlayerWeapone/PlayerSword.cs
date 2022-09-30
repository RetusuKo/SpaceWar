using UnityEngine;

public class PlayerSword : PlayerWeapon
{
    [SerializeField] private PlayerAtack _playerAtack;
    private void Awake()
    {
        Initalize(weaponSpeed: 0.5f, weaponTipe: PlayerInfo.WeaponType.Sword);
    }
    protected override void WeaponActivate()
    {
        _playerAtack.ActivateWeapone(true);
        _playerAtack.ActivateSwordAttack(_spriteRenderer);
        StartCoroutine(TimeActvateWeapone(_weaponSpeed, _playerAtack));
        _isWeaponCollDown = true;
        StartCoroutine(WeaponCoolDown(_weaponSpeed, playerIsAttacking: true));
    }
    public void Atack()
    {
        AtackConstruct("Attack");
    }
}