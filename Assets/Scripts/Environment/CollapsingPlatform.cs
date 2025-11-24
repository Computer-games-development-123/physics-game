using UnityEngine;

public class CollapsingPlatform : MonoBehaviour
{
    [Header("Collapse Timing")]
    public float delayBeforeCollapse = 0.5f;  // זמן לפני שהפלטפורמה קורסת
    public float fallSpeed = 3f;              // מהירות נפילה אחרי הקריסה

    private bool isCollapsing = false;
    private Collider2D myCollider;

    void Awake()
    {
        myCollider = GetComponent<Collider2D>();
    }

    // נרצה שהקריסה תקרה רק כשבאמת עומדים על הפלטפורמה, לכן OnCollisionStay2D
    void OnCollisionStay2D(Collision2D collision)
    {
        if (isCollapsing) return;

        if (!collision.collider.CompareTag("Player"))
            return;

        // בדיקה אם השחקן נמצא מעל הפלטפורמה (ולא מהצד/מלמטה)
        Collider2D playerCol = collision.collider;

        float playerBottom = playerCol.bounds.min.y;
        float platformCenterY = myCollider.bounds.center.y;

        bool playerAbovePlatform = playerBottom >= platformCenterY;

        // אפשר גם לוודא שהשחקן לא קופץ למעלה אלא עומד/נוחת
        Rigidbody2D playerRb = collision.rigidbody;
        bool playerFallingOrStanding = (playerRb == null) || playerRb.linearVelocity.y <= 0.1f;

        if (playerAbovePlatform && playerFallingOrStanding)
        {
            isCollapsing = true;
            StartCoroutine(CollapseRoutine());
        }
    }

    private System.Collections.IEnumerator CollapseRoutine()
    {
        // מחכים קצת לפני הקריסה
        yield return new WaitForSeconds(delayBeforeCollapse);

        // מכבים את הקוליידר כדי שלא יוכל לעמוד עליה יותר
        if (myCollider != null)
            myCollider.enabled = false;

        // מוסיפים/משתמשים ב-Rigidbody2D כדי שהפלטפורמה תיפול
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 1f;
        rb.linearVelocity = Vector2.down * fallSpeed;

        // מוחקים אחרי זמן
        Destroy(gameObject, 2f);
    }
}
