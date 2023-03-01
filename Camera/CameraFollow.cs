using UnityEngine;

public class CameraFollow : MonoBehaviour, IDatePersistance
{
    [SerializeField] private Vector2 _followOffset;
    [SerializeField] private GameObject _followObject;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _maxMinusPosition = -1f;

    private Vector2 _followThreshold;
    private Rigidbody2D _rigidBody;
    private bool _canMove = true;
    void Start()
    {
        if (_followObject == null)
        {
            _canMove = false;
            return;
        }
        _followThreshold = CalculateThreshold();
        _rigidBody = _followObject.GetComponent<Rigidbody2D>();
        _canMove = true;
    }
    void FixedUpdate()
    {
        if (_canMove)
            MoveCamera();
    }
    private void MoveCamera()
    {
        Vector2 follow = _followObject.transform.position;
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
        float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);
        Vector3 newPosition = transform.position;
        if (Mathf.Abs(xDifference) >= _followThreshold.x)
        {
            newPosition.x = follow.x;
        }
        if (Mathf.Abs(yDifference) >= _followThreshold.y)
        {
            newPosition.y = follow.y;
        }
        float moveSpeed = _rigidBody.velocity.magnitude > _speed ? _rigidBody.velocity.magnitude : _speed;
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
        if (transform.position.y <= _maxMinusPosition)
        {
            gameObject.transform.position = new Vector3(transform.position.x, _maxMinusPosition, transform.position.z);
        }
    }
    private Vector3 CalculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        float verticalSize = Camera.main.orthographicSize;
        float horizontalSize = verticalSize * aspect.width / aspect.height;
        return new Vector2(horizontalSize - _followOffset.x, verticalSize - _followOffset.y);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = CalculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }

    public void LoadDate(GameData data)
    {
        gameObject.transform.position = data.PlayerPosition + new Vector3(0, 0, -10);
    }

    public void SaveData(GameData data)
    {

    }
}