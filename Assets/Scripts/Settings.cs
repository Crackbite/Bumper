using UnityEngine;

public class Settings
{
    private readonly string _defaultNickname;

    public Settings(string defaultNickname = "Player")
    {
        _defaultNickname = defaultNickname;

        const string NicknameKey = "Nickname";
        const string TotalKillKey = "TotalKill";

        Nickname = PlayerPrefs.HasKey(NicknameKey) ? PlayerPrefs.GetString(NicknameKey) : _defaultNickname;
        TotalKill = PlayerPrefs.HasKey(TotalKillKey) ? PlayerPrefs.GetInt(TotalKillKey) : 0;
    }

    public string Nickname { get; }
    public int TotalKill { get; }

    public void SaveNickname(string nickname)
    {
        if (string.IsNullOrEmpty(nickname))
        {
            nickname = _defaultNickname;
        }

        PlayerPrefs.SetString("Nickname", nickname);
        PlayerPrefs.Save();
    }

    public void SaveTotalKill(int totalKill)
    {
        PlayerPrefs.SetInt("TotalKill", totalKill);
        PlayerPrefs.Save();
    }
}