using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PreGameScreen : Screen
{
    [SerializeField] private TMP_Text _statusLabel;
    [SerializeField] private string[] _status = { "Ready", "Steady", "GO!" };
    [SerializeField] private float _secondsBeetwenStatus = .8f;

    public event UnityAction GameStarted;

    public override void Open()
    {
        base.Open();
        StartCoroutine(ChangeStatus());
    }

    private IEnumerator ChangeStatus()
    {
        var waitForSeconds = new WaitForSeconds(_secondsBeetwenStatus);

        foreach (string status in _status)
        {
            _statusLabel.text = status;
            yield return waitForSeconds;
        }

        GameStarted?.Invoke();
    }
}