using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private float _health = 1;
    public void TakeDamage(float takenDamage)
    {
        _health -= takenDamage;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
