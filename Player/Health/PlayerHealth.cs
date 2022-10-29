using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDatePersistance
{
    [SerializeField] private float _maxHealth = 3;
    [SerializeField] private float _health = 3;

    [SerializeField] private HealthManager _healthManager;

    public float MaxHealth { get { return _maxHealth; } }
    public float Health { get { return _health; } }
    private void Awake()
    {
        _health = _maxHealth;
    }
    private void Start()
    {
        _healthManager.ShowHealthInGame();
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
        _healthManager.ShowHealthInGame();
    }
    public void Dead()
    {
        _healthManager.Dead();
    }
    private IEnumerator WaitAfterDamage()
    {
        PlayerInfo.DoNotTakeDamage = true;
        yield return new WaitForSeconds(PlayerInfo.NoDomageMiliSec);
        PlayerInfo.DoNotTakeDamage = false;
    }
    public bool HealthRegen()
    {
        if (_health < _maxHealth)
        {
            _health++;
            _healthManager.ShowHealthInGame();
            return true;
        }
        return false;
    }
    public void HealthUpgrade()
    {
        _maxHealth++;
    }
    public void LoadDate(GameData data)
    {
        _health = data.CurrentHealth;
        _maxHealth = data.MaxHealth;
        _healthManager.ShowHealthInGame();
    }

    public void SaveData(GameData data)
    {
        data.CurrentHealth = _health;
        data.MaxHealth = _maxHealth;
    }
}
