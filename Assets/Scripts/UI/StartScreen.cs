using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartScreen : Screen
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_InputField _nickname;
    [SerializeField] private TMP_Text _totalKill;
    [SerializeField] private Button _playButton;

    public event UnityAction PlayButtonClicked;

    public override void Open()
    {
        _nickname.text = _player.Nickname;
        _totalKill.text = _player.TotalKill.ToString();

        _nickname.interactable = true;
        base.Open();
    }

    public override void Close()
    {
        base.Close();
        _nickname.interactable = false;
    }

    public void OnPlayButtonClick()
    {
        _player.SetNickname(_nickname.text);
        PlayButtonClicked?.Invoke();
    }

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnPlayButtonClick);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClick);
    }
}