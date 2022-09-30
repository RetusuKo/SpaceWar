using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float _health = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamageEnemy(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        TakeDamageEnemy(collision);
    }

    private void TakeDamageEnemy(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !PlayerInfo.IsAttacking && !PlayerInfo.DoNotTakeDamage)
        {
            TakeDamage(collision.gameObject.GetComponent<Enemy>().Damage);
            print(_health);
        }
        if (collision.tag == "EnemyGiveDamage" && !PlayerInfo.IsAttacking && !PlayerInfo.DoNotTakeDamage)
        {
            TakeDamage(collision.gameObject.GetComponent<EneemyBullet>().Damage);
            print(_health);
        }
    }
    private void TakeDamage(float damage)
    {

        _health -= damage;
        StartCoroutine(WaitAfterDDamage());
        Player player = GetComponent<Player>();
        if (_health >= 1)
            player.Hurt();
        else if (_health <= 0)
            player.Dead();
    }
    private IEnumerator WaitAfterDDamage()
    {
        PlayerInfo.DoNotTakeDamage = true;
        yield return new WaitForSeconds(PlayerInfo.NoDomageMiliSec);
        PlayerInfo.DoNotTakeDamage = false;
    }
}
