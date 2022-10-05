using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour, IDatePersistance
{
    [SerializeField] private Vector2 _followOffset;
    [SerializeField] private GameObject _followObject;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _maxMinusPositio = -1f;

    private Vector2 _threshold;
    private Rigidbody2D _rigidBody;
    void Start()
    {
        _threshold = CalculateThreshold();
        _rigidBody = _followObject.GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Vector2 follow = _followObject.transform.position;
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
        float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);
        Vector3 newPosition = transform.position;
        if (Mathf.Abs(xDifference) >= _threshold.x)
        {
            newPosition.x = follow.x;
        }
        if (Mathf.Abs(yDifference) >= _threshold.y)
        {
            newPosition.y = follow.y;
        }
        float moveSpeed = _rigidBody.velocity.magnitude > _speed ? _rigidBody.velocity.magnitude : _speed;
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
        if (transform.position.y <= _maxMinusPositio)
        {
            gameObject.transform.position = new Vector3(transform.position.x, _maxMinusPositio, transform.position.z);
        }
    }
    private Vector3 CalculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= _followOffset.x;
        t.y -= _followOffset.y;
        return t;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = CalculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }

    public void LoadDate(GameData data)
    {
        gameObject.transform.position = data.PlayerPosition + new Vector3(0,0, -10);
    }

    public void SaveData(GameData data)
    {

    }
}
