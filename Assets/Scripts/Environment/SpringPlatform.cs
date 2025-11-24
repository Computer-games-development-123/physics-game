using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpringPlatform : MonoBehaviour
{
    [Header("Boost Settings")]
    public float boostJumpForce = 18f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        Rigidbody2D playerRb = collision.collider.attachedRigidbody;
        if (playerRb == null)
            return;

        Transform player = collision.collider.transform;

        bool isAbove = player.position.y > transform.position.y;
        bool isFallingOrStill = playerRb.linearVelocity.y <= 0.1f;

        if (isAbove && isFallingOrStill)
        {
            Vector2 v = playerRb.linearVelocity;
            v.y = 0f;
            playerRb.linearVelocity = v;

            playerRb.AddForce(Vector2.up * boostJumpForce, ForceMode2D.Impulse);
        }
    }
}
