using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathZone : MonoBehaviour
{
    [Tooltip("Player's Tag")]
    public string playerTag = "Player";

    [Tooltip("The Game-Over-Panel that will appear when falling")]
    public GameObject gameOverPanel;
    public PlayerMovement playerMovement;
    public ScoreManager scoreManager;
    public TextMeshProUGUI finalScoreText;
    private bool isGameOver = false;

    void Start()
    {
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

    if (playerMovement != null)
        playerMovement.enabled = false;

    Time.timeScale = 0f;

    if (gameOverPanel != null)
        gameOverPanel.SetActive(true);

    if (scoreManager != null && finalScoreText != null)
    {
        finalScoreText.text = "Final Score: " + scoreManager.CurrentScore;
    }
}


    public void RestartLevel()
    {
        Time.timeScale = 1f;

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }
}
