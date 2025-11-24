using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float maxMoveSpeed = 8f;       // מהירות מקסימלית
    public float moveAcceleration = 20f;  // קצב האצה
    public float jumpForce = 16f;

    [Header("Momentum Jump Settings")]
    public float maxSpeedForBonus = 10f;
    public float extraJumpForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

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
            // מהירות אופקית נוכחית (תנופה)
            float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);

            // ננרמל את המהירות לטווח [0,1] לפי maxSpeedForBonus
            float t = Mathf.InverseLerp(0f, maxSpeedForBonus, horizontalSpeed);

            // מחשבים בונוס קפיצה
            float bonus = t * extraJumpForce;

            // כוח קפיצה סופי
            float finalJumpForce = jumpForce + bonus;

            // מאפסים מהירות Y כדי שהקפיצה תהיה נקייה
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            // מוסיפים כוח קפיצה עם הבונוס
            rb.AddForce(Vector2.up * finalJumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput * maxMoveSpeed;
        float currentX = rb.linearVelocity.x;

        float newX = Mathf.MoveTowards(
            currentX,
            targetSpeed,
            moveAcceleration * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
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
