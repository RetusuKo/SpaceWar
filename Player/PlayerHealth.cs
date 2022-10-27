using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDatePersistance
{
    [SerializeField] private float _health = 3;

    [SerializeField] private List<Image> _healtImage;

    private void Start()
    {
        for (int i = 0; i < _healtImage.Count; i++)
            if (i < _health)
                _healtImage[i].gameObject.SetActive(true);
    }
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
        StartCoroutine(WaitAfterDamage());
        Player player = GetComponent<Player>();
        if (_health >= 1)
            player.Hurt();
        else if (_health <= 0)
            player.Dead();
        _healtImage[(int)_health].enabled = false;
    }
    private IEnumerator WaitAfterDamage()
    {
        PlayerInfo.DoNotTakeDamage = true;
        yield return new WaitForSeconds(PlayerInfo.NoDomageMiliSec);
        PlayerInfo.DoNotTakeDamage = false;
    }

    public void LoadDate(GameData data)
    {
        _health = data.Health;
    }

    public void SaveData(GameData data)
    {
        data.Health = _health;
    }
}
