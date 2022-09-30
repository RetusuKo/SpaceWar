using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{
    [SerializeField] private GameObject _atackRadius;
    private void Awake()
    {
        Initalize(speed: 3, damage: 1, health: 1);
    }
    public override void TakeDamage(float takenDamage)
    {
        _health -= takenDamage;
        if (_health <= 0)
        {
            Destroy(gameObject);
            Destroy(_atackRadius);
        }
    }
}
