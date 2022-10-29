using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private List<Health> _healths;
    [SerializeField] private PlayerHealth _playerHealth;
    public void ShowHealthInGame()
    {
        for (int i = 0; i < _healths.Count; i++)
        {
            if (i < _playerHealth.Health)
            {
                _healths[i].gameObject.SetActive(true);
            }
            else
            {
                _healths[i].gameObject.SetActive(false);
            }
        }
    }
    public void Dead()
    {
        for (int i = 0; i < _healths.Count; i++)
        {
            _healths[i].gameObject.SetActive(false);
        }
    }
}
