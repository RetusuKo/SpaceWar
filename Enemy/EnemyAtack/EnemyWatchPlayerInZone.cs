using System.Collections;
using UnityEngine;

public class EnemyWatchPlayerInZone : MonoBehaviour
{
    [SerializeField] private EnemyGun _enemyGun;
    [Header("OverlapBox parametrs")]
    [SerializeField] private Transform _detectorOrigin;
    [SerializeField] private Vector2 _detectorSize = Vector2.one;
    [SerializeField] private Vector2 _detectorOriginalOffset = Vector2.zero;
    [SerializeField] private LayerMask _detectorLayerMask;
    [SerializeField] private float _detectionDelay = 0.3f;


    private Color _gizmoIdleColor = new Color(0, 1, 0, 0.2f);
    private Color _gizmoDetectedColor = new Color(1, 0, 0, 0.2f);

    private bool _showGizmo = true;
    private bool _isAtacking = false;
    private bool _playerDetected;

    private GameObject _target;
    private void Start()
    {
        StartCoroutine(DetectionCoroutine());
    }
    private IEnumerator DetectionCoroutine()
    {
        Atack();
        yield return new WaitForSeconds(_detectionDelay);
        PerormDetection();
        StartCoroutine(DetectionCoroutine());
    }
    private IEnumerator AtackAfterTime()
    {
        yield return new WaitForSeconds(1.3f);
        _enemyGun.Atack();
        _isAtacking = false;
    }
    private void Atack()
    {
        if (_playerDetected && !_isAtacking)
        {
            _isAtacking = true;
            StartCoroutine(AtackAfterTime());
        }
    }
    private void PerormDetection()
    {
        Collider2D collider = Physics2D.OverlapBox((Vector2)_detectorOrigin.position + _detectorOriginalOffset, _detectorSize, 0, _detectorLayerMask);
        if (collider != null)
        {
            _target = collider.gameObject;
            _playerDetected = true;
        }
        else
        {
            _target = null;
            _playerDetected = false;
        }
    }
    private void OnDrawGizmos()
    {
        if (_showGizmo && _detectorOrigin != null)
        {
            Gizmos.color = _gizmoIdleColor;
            if (_playerDetected)
                Gizmos.color = _gizmoDetectedColor;
            Gizmos.DrawCube((Vector2)_detectorOrigin.position + _detectorOriginalOffset, _detectorSize);
        }
    }
}