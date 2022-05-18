using UnityEngine;

public class Settings
{
    private readonly string _defaultNickname;

    public Settings(string defaultNickname = "Player")
    {
        _defaultNickname = defaultNickname;
        Nickname = PlayerPrefs.HasKey("Nickname") ? PlayerPrefs.GetString("Nickname") : _defaultNickname;
        TotalKill = PlayerPrefs.HasKey("TotalKill") ? PlayerPrefs.GetInt("TotalKill") : 0;
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