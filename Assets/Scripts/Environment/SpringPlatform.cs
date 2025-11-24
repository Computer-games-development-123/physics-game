using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpringPlatform : MonoBehaviour
{
    [Header("Boost Settings")]
    public float boostJumpForce = 18f;   // אפשר לשחק עם הערך עד שמרגיש טוב

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // נטפל רק בשחקן
        if (!collision.collider.CompareTag("Player"))
            return;

        Rigidbody2D playerRb = collision.collider.attachedRigidbody;
        if (playerRb == null)
            return;

        Transform player = collision.collider.transform;

        // תנאי "נחיתה מלמעלה":
        // 1. השחקן גבוה יותר מהפלטפורמה
        // 2. המהירות שלו בכיוון Y כלפי מטה (או כמעט 0)
        bool isAbove = player.position.y > transform.position.y;
        bool isFallingOrStill = playerRb.linearVelocity.y <= 0.1f;

        if (isAbove && isFallingOrStill)
        {
            // מאפסים מהירות Y כדי שהBoost יהיה נקי
            Vector2 v = playerRb.linearVelocity;
            v.y = 0f;
            playerRb.linearVelocity = v;

            // מוסיפים כוח קפיצה חזק למעלה
            playerRb.AddForce(Vector2.up * boostJumpForce, ForceMode2D.Impulse);
        }
    }
}
