using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private float _speed = 20;
    private float _damage = 0.5f;

    private float _timeeAfterDestroy = 2f;
    private Rigidbody2D _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        ShootBullet();
    }
    public void ShootBullet()
    {
        if (PlayerInfo.WatchRight)
            _rigidbody.velocity = transform.right * _speed;
        else
            _rigidbody.velocity = transform.right * -_speed;
        StopAwterLenght();
    }

    private void StopAwterLenght()
    {
        StartCoroutine(WaitDestroyTime());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Wall wall = collision.gameObject.GetComponent<Wall>();
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (collision.tag == "Enemy")
        {
            enemy.TakeDamage(_damage);
            Destroy(gameObject);
        }
        else if (collision.tag == "BlowUp")
        {
            wall.TakeDamage(_damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator WaitDestroyTime()
    {
        yield return new WaitForSeconds(_timeeAfterDestroy);
        Destroy(gameObject);
    }
}
