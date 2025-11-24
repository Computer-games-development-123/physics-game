using UnityEngine;
using TMPro; // אם אתה משתמש ב-TextMeshPro

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;             // השחקן
    public TextMeshProUGUI scoreText;    // טקסט הניקוד על המסך

    [Header("Score Settings")]
    public float heightToPointsFactor = 5f; // כמה נקודות לכל יחידת גובה

    private float highestY = 0f;
    private int currentScore = 0;

    // נרצה לגשת לניקוד גם ממקומות אחרים (למשל במסך Game Over)
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

        // אם השחקן עלה יותר גבוה מהשיא הקודם
        if (player.position.y > highestY)
        {
            highestY = player.position.y;

            // המרה מנקודות גובה לניקוד שלם
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
