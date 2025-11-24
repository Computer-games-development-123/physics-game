using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathZone : MonoBehaviour
{
    [Tooltip("Tag של השחקן")]
    public string playerTag = "Player";

    [Tooltip("פאנל ה-Game Over שיופיע כשנופלים")]
    public GameObject gameOverPanel;

    [Tooltip("סקריפט התנועה של השחקן (לא חובה למלא ידנית - אפשר למצוא אוטומטית)")]
    public PlayerMovement playerMovement;
    public ScoreManager scoreManager;       // הפניה למנהל הניקוד
    public TextMeshProUGUI finalScoreText;  // טקסט למסך ה-Game Over
    private bool isGameOver = false;

    void Start()
    {
        // לוודא שהפאנל כבוי בתחילת המשחק
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOver) return;

        if (other.CompareTag(playerTag))
        {
            HandlePlayerDeath(other.gameObject);
        }
    }

    void HandlePlayerDeath(GameObject player)
{
    isGameOver = true;

    if (playerMovement == null)
        playerMovement = player.GetComponent<PlayerMovement>();

    // לכבות תנועה
    if (playerMovement != null)
        playerMovement.enabled = false;

    // לעצור זמן במשחק
    Time.timeScale = 0f;

    // להפעיל את מסך ה-GameOver
    if (gameOverPanel != null)
        gameOverPanel.SetActive(true);

    // הצגת ניקוד סופי
    if (scoreManager != null && finalScoreText != null)
    {
        finalScoreText.text = "Final Score: " + scoreManager.CurrentScore;
    }
}


    // פונקציה שאליה הכפתור יקרא
    public void RestartLevel()
    {
        // להחזיר את הזמן לקדמותו
        Time.timeScale = 1f;

        // לטעון את הסצנה מחדש
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }
}
