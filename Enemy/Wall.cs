using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private float _health = 1;

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
