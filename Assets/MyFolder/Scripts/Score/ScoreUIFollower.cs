using UnityEngine;
using TMPro;

public class ScoreUIFollower : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Update()
    {
        if (ScoreManager.Instance != null)
        {
            scoreText.text = $"SCORE : {ScoreManager.Instance.GetScore()}";
        }
    }
}
