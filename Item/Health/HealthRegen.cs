using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegen : Item
{
    [SerializeField] private PlayerHealth _playerHealth;
    protected override void TakeItem(Collider2D collision)
    {
        if (_playerHealth.HealthRegen())
            AfterTakeObject(collision);
    }
}
