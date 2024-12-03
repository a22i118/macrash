using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private bool _isGameStart = false;
    private bool _isPlayerSet = true;
    private List<int> _scoreNum = new List<int>();
    private List<TextMeshProUGUI> _scores = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> Scores { get => _scores; set => _scores = value; }
    public bool IsGameStart { get => _isGameStart; set => _isGameStart = value; }

    void Start()
    {

    }
    void Update()
    {
        if (_isGameStart)
        {
            if (_isPlayerSet)
            {
                InitializeScore();
                _isPlayerSet = false;
            }
        }
    }
    private void InitializeScore()
    {
        for (int i = 0; i < _scores.Count; i++)
        {
            _scoreNum.Add(0);
            _scores[i].text = _scoreNum[i].ToString();
        }
    }
    public void UpdateScore(string player)
    {
        int playerIndex = -1;

        switch (player)
        {
            case "Player (1)":
                playerIndex = 0;
                break;
            case "Player (2)":
                playerIndex = 1;
                break;
            case "Player (3)":
                playerIndex = 2;
                break;
            case "Player (4)":
                playerIndex = 3;
                break;
        }
        if (playerIndex >= 0 && playerIndex < _scoreNum.Count)
        {
            _scoreNum[playerIndex]++;
            _scores[playerIndex].text = _scoreNum[playerIndex].ToString();
        }
    }
}
