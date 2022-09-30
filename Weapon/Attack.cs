using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] protected Transform _shootPosition;
    [SerializeField] protected GameObject _shooter;

    protected BoxCollider2D _boxCollider;

    protected float _damage = 1;
    protected void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }
    public void ActivateSwordAttack(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer.flipX)
            _boxCollider.offset = new Vector2(-2.7f, _boxCollider.offset.y);
        else if (!spriteRenderer.flipX)
            _boxCollider.offset = new Vector2(1.2f, _boxCollider.offset.y);
        gameObject.transform.position = new Vector2(_shootPosition.position.x, _shootPosition.position.y);
    }
    public void ActivateGunAttack(SpriteRenderer spriteRenderer, GameObject bulletPrefab)
    {
        if (spriteRenderer.flipX)
        {
            PlayerInfo.WatchRight = false;
            Instantiate(bulletPrefab, _shootPosition.position - new Vector3(2f, (gameObject.transform.position.y - _shooter.transform.position.y) - 1.35f), _shootPosition.rotation);
        }
        else if (!spriteRenderer.flipX)
        {
            PlayerInfo.WatchRight = true;
            Instantiate(bulletPrefab, _shootPosition.position, _shootPosition.rotation);
        }
    }
    public void ActivateGunEnemyAttack(SpriteRenderer spriteRenderer, GameObject bulletPrefab)
    {
        if (spriteRenderer.flipX)
        {
            EnemyInfo.WatchRight = false;
            Instantiate(bulletPrefab, _shootPosition.position, _shootPosition.rotation);
        }
        else if (!spriteRenderer.flipX)
        {
            EnemyInfo.WatchRight = true;
            Instantiate(bulletPrefab, _shootPosition.position, _shootPosition.rotation);
        }
    }
    public void ActivateWeapone(bool eneble)
    {
        gameObject.SetActive(eneble);
    }
}