using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtack : Attack
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        Wall wall = collision.gameObject.GetComponent<Wall>();
        if (collision.tag == "Enemy" && !PlayerInfo.DoNotTakeDamage)
        {
            enemy.TakeDamage(_damage);
            PlayerInfo.IsAttacking = true;
        }
        if (collision.tag == "EnemyWall")
        {
            wall.TakeDamage(_damage);
        }
    }
}