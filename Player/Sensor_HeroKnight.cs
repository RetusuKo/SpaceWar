using UnityEngine;
using System.Collections;

public class Sensor_HeroKnight : MonoBehaviour {

    private int _colCount = 0;

    private float _disableTimer;

    private void OnEnable()
    {
        _colCount = 0;
    }

    public bool State()
    {
        if (_disableTimer > 0)
            return false;
        return _colCount > 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        _colCount++;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        _colCount--;
    }

    void Update()
    {
        _disableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        _disableTimer = duration;
    }
}
