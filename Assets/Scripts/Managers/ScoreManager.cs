using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public TextMeshProUGUI scoreText;

    [Header("Score Settings")]
    public float heightToPointsFactor = 5f;

    private float highestY = 0f;
    private int currentScore = 0;

    public int CurrentScore => currentScore;

    void Start()
    {
        if (player != null)
        {
            highestY = player.position.y;
        }

        UpdateScoreText();
    }

    void Update()
    {
        if (player == null) return;

        if (player.position.y > highestY)
        {
            highestY = player.position.y;
            currentScore = Mathf.Max(0, Mathf.RoundToInt(highestY * heightToPointsFactor));

            UpdateScoreText();
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
}
