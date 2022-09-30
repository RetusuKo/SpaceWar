using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected bool _isWeaponCollDown = false;
    protected float _weaponSpeed;

    protected void GetAllComponent()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    protected IEnumerator TimeActvateWeapone(float time, PlayerAtack playerAtack = null, EnemyAtack enemyAtack = null)
    {
        yield return new WaitForSeconds(time);
        if (playerAtack == null)
        {
            enemyAtack.ActivateWeapone(true);
        }
        else
        {
            playerAtack.ActivateWeapone(false);
        }
    }
    protected IEnumerator WeaponCoolDown(float time, bool waponCoolDown = true, bool playerIsAttacking = false)
    {
        yield return new WaitForSeconds(time);
        if (_isWeaponCollDown)
        {
            _isWeaponCollDown = false;
            if (playerIsAttacking)
            {
                PlayerInfo.IsAttacking = false;
            }
        }
    }
}
