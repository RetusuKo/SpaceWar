using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : Item
{
    [SerializeField] private PlayerHealth _playerHealth;
    protected override void TakeItem(Collider2D collision)
    {
        _playerHealth.HealthUpgrade();
        AfterTakeObject(collision);
    }
}
