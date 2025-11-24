using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 16f;

    [Header("Ground Check")]
    public Transform groundCheck;        // נגרור את ה-GroundCheck לכאן
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;        // נגדיר ל-Ground את הלייר הזה

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // קלט צדדי (חץ ימין/שמאל או A/D)
        moveInput = Input.GetAxisRaw("Horizontal");

        // בדיקת קרקע
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // קפיצה – רק אם על הקרקע
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // מאפס Y כדי לקפוץ נקי
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(moveInput) > 0.01f)
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
    }

    void OnDrawGizmosSelected()
    {
        // סתם כדי לראות את רדיוס הקרקע בסצנה
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
