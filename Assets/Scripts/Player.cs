using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover), typeof(Rigidbody))]
public class Player : Character
{
    private int _kill;
    private PlayerMover _mover;
    private int _rank;
    private Settings _settings;

    public event UnityAction Won;
    public event UnityAction Reset;
    public event UnityAction<int> KillChanged;
    public event UnityAction<int> RankChanged;

    public int TotalKill { get; private set; }

    public void SetNickname(string nickname)
    {
        _settings.SaveNickname(nickname);
        Nickname = nickname;
    }

    public void AddToTotalKill(int kill)
    {
        _settings.SaveTotalKill(TotalKill + kill);
        TotalKill += kill;
    }

    public void SetInitialRank(int rank)
    {
        _rank = rank + 1;
        RankChanged?.Invoke(_rank);
    }

    public void IncreaseKill()
    {
        _kill++;
        KillChanged?.Invoke(_kill);
    }

    public void DecreaseRank()
    {
        _rank--;
        RankChanged?.Invoke(_rank);
    }

    public void ResetPlayer()
    {
        _kill = 0;
        KillChanged?.Invoke(_kill);
        IsDied = false;

        gameObject.SetActive(false);
        Rigidbody.velocity = Vector3.zero;
        _mover.ResetMover();
        gameObject.SetActive(true);

        Reset?.Invoke();
    }

    public void Win()
    {
        Won?.Invoke();
    }

    private void Start()
    {
        _settings = new Settings();
        Nickname = _settings.Nickname;
        TotalKill = _settings.TotalKill;

        _mover = GetComponent<PlayerMover>();
    }
}