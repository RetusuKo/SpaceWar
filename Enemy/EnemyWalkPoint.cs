using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkPoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (gameObject.name == "WalkPoint1")
            {
                _spriteRenderer.flipX = false;
            }
            else if (gameObject.name == "WalkPoint2")
            {
                _spriteRenderer.flipX = true;
            }
        }
    }
}
