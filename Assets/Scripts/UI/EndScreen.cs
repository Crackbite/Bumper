using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EndScreen : Screen
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _victoryLabel;
    [SerializeField] private TMP_Text _defeatLabel;
    [SerializeField] private TMP_Text _rank;
    [SerializeField] private TMP_Text _kill;
    [SerializeField] private TMP_Text _killedBy;
    [SerializeField] private Button _continueButton;
    [SerializeField] private ParticleSystem _fireworks;

    public event UnityAction ContinueButtonClicked;
    public event UnityAction GameEnded;

    public void OnContinueButtonClicked()
    {
        ContinueButtonClicked?.Invoke();
    }

    public void OpenVictory()
    {
        _defeatLabel.gameObject.SetActive(false);
        _killedBy.gameObject.SetActive(false);
        _victoryLabel.gameObject.SetActive(true);
        base.Open();

        Instantiate(_fireworks, _player.transform.position, Quaternion.identity);
        StartCoroutine(PreGameEnd());
    }

    public void OpenDefeat()
    {
        _victoryLabel.gameObject.SetActive(false);
        _defeatLabel.gameObject.SetActive(true);

        base.Open();
        GameEnded?.Invoke();
    }

    private void OnEnable()
    {
        _player.RankChanged += OnPlayerRankChanged;
        _player.KillChanged += OnPlayerKillChanged;
        _player.Died += OnPlayerDied;
        _continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    private void OnDisable()
    {
        _player.RankChanged -= OnPlayerRankChanged;
        _player.KillChanged -= OnPlayerKillChanged;
        _player.Died -= OnPlayerDied;
        _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
    }

    private void OnPlayerRankChanged(int rank)
    {
        _rank.text = rank.ToString();
    }

    private IEnumerator PreGameEnd()
    {
        yield return new WaitForSeconds(_fireworks.main.startLifetime.constantMax);
        GameEnded?.Invoke();
    }

    private void OnPlayerKillChanged(int kill)
    {
        _kill.text = kill.ToString();
    }

    private void OnPlayerDied(Character character, GameObject lastGameObjectCollision)
    {
        _player.AddToTotalKill(Convert.ToInt16(_kill.text));

        if (lastGameObjectCollision != null && lastGameObjectCollision.TryGetComponent(out Enemy enemy))
        {
            _killedBy.text = $"Killed by {enemy.Nickname}";
            _killedBy.gameObject.SetActive(true);
        }
        else
        {
            _killedBy.gameObject.SetActive(false);
        }
    }
}