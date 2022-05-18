using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private TMP_Text _nickname;

    private readonly List<string> _nicknameList = new List<string>
    {
        "Jacob",
        "Emily",
        "Michael",
        "Emma",
        "Joshua",
        "Madison",
        "Matthew",
        "Olivia",
        "Ethan",
        "Isabella"
    };

    private void Start()
    {
        int randomNumber = Random.Range(10, 99);
        int randomIndex = Random.Range(0, _nicknameList.Count);

        Nickname = $"{_nicknameList[randomIndex]}{randomNumber}";
        _nickname.text = Nickname;
    }
}