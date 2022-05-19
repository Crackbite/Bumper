using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed = 2.5f;
    [SerializeField] private int _rotateSpeed = 300;
    [SerializeField] private PreGameScreen _preGameScreen;

    private bool _isGameStart;
    private PlayerInput _playerInput;
    private Vector3 _startlocalScale;
    private Vector3 _startPosition;

    public void OnGameStarted()
    {
        _isGameStart = true;
    }

    public void ResetMover()
    {
        _isGameStart = false;
        transform.localScale = _startlocalScale;
        transform.SetPositionAndRotation(_startPosition, Quaternion.Euler(0, 0, 0));
    }

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void Start()
    {
        _startPosition = transform.position;
        _startlocalScale = transform.localScale;
    }

    private void Update()
    {
        if (_isGameStart)
        {
            transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
        }

        var moveDirection = _playerInput.Default.Move.ReadValue<Vector2>();

        if (moveDirection.sqrMagnitude < 0.1)
        {
            return;
        }

        var movementDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
        Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotateSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _preGameScreen.GameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _preGameScreen.GameStarted -= OnGameStarted;
    }
}