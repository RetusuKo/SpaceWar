using UnityEngine;

public class Walker : Enemy
{
    private void Awake()
    {
        Initalize(speed:3, damage: 1, health: 1);
    }
    private void FixedUpdate()
    {
        Movement();
    }
    protected /*override*/ void Movement()
    {
        _rigidbody.velocity = new Vector2(_speed * Direction(), _rigidbody.velocity.y);
        _animator.SetInteger("SpeedX", (int)_rigidbody.velocity.x);
    }

    public override void TakeDamage(float takenDamage)
    {
        _health -= takenDamage;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
