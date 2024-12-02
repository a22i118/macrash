using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int[] Score = new int[4];
    // スコアを表示するテキスト
    [SerializeField] private TextMeshProUGUI _score1;
    [SerializeField] private TextMeshProUGUI _score2;
    [SerializeField] private TextMeshProUGUI _score3;
    [SerializeField] private TextMeshProUGUI _score4;

    void Start()
    {
        InitializeScore();
    }

    // Update is called once per frame
    // スコアの初期化
    private void InitializeScore()
    {
        foreach (int i in Score)
        {
            Score[i] = 0;
        }
        _score1.text = $"{Score[0]}";
        _score2.text = $"{Score[1]}";
        _score3.text = $"{Score[2]}";
        _score4.text = $"{Score[3]}";
    }

    //スコアの加算
    public void UpdateScore(string player)
    {
        switch (player)
        {
            case "Player (1)":
                Score[0]++;
                _score1.text = $"{Score[0]}";
                break;
            case "Player (2)":
                Score[1]++;
                _score2.text = $"{Score[1]}";
                break;
            case "Player (3)":
                Score[2]++;
                _score3.text = $"{Score[2]}";
                break;
            case "Player (4)":
                Score[3]++;
                _score4.text = $"{Score[3]}";
                break;
        }
    }
}
