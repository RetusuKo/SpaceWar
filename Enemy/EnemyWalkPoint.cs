using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkPoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private static readonly Dictionary<string, bool> _walkPointDirections = new Dictionary<string, bool>()
    {
        {"WalkPoint1", false},
        {"WalkPoint2", true},
    };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && _walkPointDirections.TryGetValue(gameObject.name, out bool flipX))
        {
            _spriteRenderer.flipX = flipX;
        }
    }
}
