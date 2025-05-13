using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using ViveSR.anipal.Eye;

public class ResultManager : MonoBehaviour
{
    public GameObject resultPanel;
    public GameObject rankingPanel;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI rankingListText;

    private bool isBlinking = false;
    private float blinkTimer = 0f;
    private float blinkThreshold = 0.5f;

    private const int MaxRankings = 100;
    private const string RankingKey = "HighScores";

    private bool resultShown = false;
    private bool rankingShown = false;

    void Update()
    {
        if (!resultShown || rankingShown) return;

        var shooter = BlinkShooter.Instance;
        if (shooter != null && shooter.IsBothEyesClosed && shooter.BlinkDuration >= 0.5f)
        {
            ShowRanking();
        }
    }


    public void ShowResult(int score, int maxCombo)
    {
        resultShown = true;

        resultPanel.SetActive(true);
        rankingPanel.SetActive(false);

        scoreText.text = $"SCORE: {score}";
        comboText.text = $"MAX COMBO: {maxCombo}";
        rankText.text = $"RANK: {GetRank(score)}";

        SaveScore(score);
    }

    void ShowRanking()
    {
        rankingShown = true;
        resultPanel.SetActive(false);
        rankingPanel.SetActive(true);

        List<int> scores = LoadRankings();
        int currentScore = ScoreManager.Instance.GetScore();

        rankingListText.text = "";
        for (int i = 0; i < Mathf.Min(10, scores.Count); i++)
        {
            int score = scores[i];
            string color = (score == currentScore) ? "<color=red>" : "";
            string endColor = (score == currentScore) ? "</color>" : "";
            rankingListText.text += $"{i + 1}. {color}{score}{endColor}\n";
        }
    }


    string GetRank(int score)
    {
        List<int> scores = LoadRankings();
        scores.Add(score);
        scores = scores.OrderByDescending(s => s).ToList();

        int rank = scores.IndexOf(score) + 1;

        if (rank <= 5) return "SS";
        if (rank <= 10) return "S";
        if (rank <= 30) return "A";
        if (rank <= 100) return "B";
        return "C";
    }


    void SaveScore(int score)
    {
        List<int> scores = LoadRankings();
        scores.Add(score);
        scores = scores.OrderByDescending(s => s).Take(MaxRankings).ToList();

        string save = string.Join(",", scores);
        PlayerPrefs.SetString(RankingKey, save);
        PlayerPrefs.Save();
    }

    List<int> LoadRankings()
    {
        string raw = PlayerPrefs.GetString(RankingKey, "");
        return raw.Split(',')
                  .Where(s => int.TryParse(s, out _))
                  .Select(int.Parse)
                  .ToList();
    }
}
