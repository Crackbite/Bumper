using TMPro;
using UnityEngine;

public class GameScreen : Screen
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _kill;

    private void OnEnable()
    {
        _player.KillChanged += OnPlayerKillChanged;
    }

    private void OnDisable()
    {
        _player.KillChanged -= OnPlayerKillChanged;
    }

    private void OnPlayerKillChanged(int kill)
    {
        _kill.text = kill.ToString();
    }
}