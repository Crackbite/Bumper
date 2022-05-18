using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _smoothFactor = 0.2f;

    private bool _isStopFollowing;
    private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position - _player.transform.position;
    }

    private void LateUpdate()
    {
        if (_isStopFollowing)
        {
            return;
        }

        Vector3 newPosition = _player.transform.position + _offset;
        transform.position = Vector3.Slerp(transform.position, newPosition, _smoothFactor);
    }

    private void OnEnable()
    {
        _player.Died += OnPlayerDied;
        _player.Reset += OnPlayerReset;
    }

    private void OnDisable()
    {
        _player.Died -= OnPlayerDied;
        _player.Reset -= OnPlayerReset;
    }

    private void OnPlayerReset()
    {
        _isStopFollowing = false;
    }

    private void OnPlayerDied(Character character, GameObject lastGameObjectCollision)
    {
        _isStopFollowing = true;
    }
}