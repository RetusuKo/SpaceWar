using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EneemyBullet : MonoBehaviour
{
    private float _speed = 20;
    private float _damage = 1;
    private Rigidbody2D _rigidbody;
    public float Damage
    {
        get { return _damage; }
    }
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
        if (EnemyInfo.WatchRight)
            _rigidbody.velocity = transform.right * _speed;
        else
           _rigidbody.velocity = transform.right * -_speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
