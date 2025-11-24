using UnityEngine;

public class CollapsingPlatform : MonoBehaviour
{
    [Header("Collapse Timing")]
    public float delayBeforeCollapse = 0.5f;
    public float fallSpeed = 3f;

    private bool isCollapsing = false;
    private Collider2D myCollider;

    void Awake()
    {
        myCollider = GetComponent<Collider2D>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isCollapsing) return;

        if (!collision.collider.CompareTag("Player"))
            return;

        Collider2D playerCol = collision.collider;

        float playerBottom = playerCol.bounds.min.y;
        float platformCenterY = myCollider.bounds.center.y;

        bool playerAbovePlatform = playerBottom >= platformCenterY;

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
        yield return new WaitForSeconds(delayBeforeCollapse);

        if (myCollider != null)
            myCollider.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 1f;
        rb.linearVelocity = Vector2.down * fallSpeed;

        Destroy(gameObject, 2f);
    }
}
