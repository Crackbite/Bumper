using UnityEngine;

[RequireComponent(typeof(EnemySpawner))]
public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Platform _platform;
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private PreGameScreen _preGameScreen;
    [SerializeField] private GameScreen _gameScreen;
    [SerializeField] private EndScreen _endScreen;

    private EnemySpawner _enemySpawner;

    private void Start()
    {
        _enemySpawner = GetComponent<EnemySpawner>();
        _startScreen.Open();
    }

    private void OnEnable()
    {
        _startScreen.PlayButtonClicked += OnPlayButtonClicked;
        _player.Died += OnPlayerDied;
        _player.Won += OnPlayerWon;
        _preGameScreen.GameStarted += OnGameStarted;
        _endScreen.GameEnded += OnGameEnded;
        _endScreen.ContinueButtonClicked += OnContinueButtonClicked;
    }

    private void OnDisable()
    {
        _startScreen.PlayButtonClicked -= OnPlayButtonClicked;
        _player.Died -= OnPlayerDied;
        _player.Won -= OnPlayerWon;
        _preGameScreen.GameStarted -= OnGameStarted;
        _endScreen.GameEnded -= OnGameEnded;
        _endScreen.ContinueButtonClicked -= OnContinueButtonClicked;
    }

    private void OnPlayerWon()
    {
        _gameScreen.Close();
        _endScreen.OpenVictory();
    }

    private void OnContinueButtonClicked()
    {
        _platform.ResetPlatform();
        _player.ResetPlayer();
        _enemySpawner.DestroySpawnEnemies();
        _endScreen.Close();
        _startScreen.Open();
        Time.timeScale = 1;
    }

    public void OnPlayButtonClicked()
    {
        _startScreen.Close();
        _preGameScreen.Open();
    }

    public void OnGameStarted()
    {
        _enemySpawner.Spawn();
        _preGameScreen.Close();
        _gameScreen.Open();
        _platform.StartChangeSize();
    }

    private void OnGameEnded()
    {
        Time.timeScale = 0;
    }

    private void OnPlayerDied(Character player, GameObject lastGameObjectCollision)
    {
        _gameScreen.Close();
        _endScreen.OpenDefeat();
    }
}