using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;
    protected BoxCollider2D _boxCollider;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

    protected float _health;
    protected float _speed;
    protected float _damage;

    public float Damage => _damage;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        gameObject.tag = "Enemy";
        gameObject.layer = LayerMask.NameToLayer("Enemy"); ;
    }
    public abstract void TakeDamage(float takenDamage);
    protected void Initalize(float speed, float damage, float health)
    {
        _speed = speed;
        _damage = damage;
        _health = health;
        Awake();
    }
    protected int Direction()
    {
        return _spriteRenderer.flipX ? -1 : 1;
    }
}