using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float maxMoveSpeed = 8f;
    public float moveAcceleration = 20f;
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
        moveInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);

            float t = Mathf.InverseLerp(0f, maxSpeedForBonus, horizontalSpeed);

            float bonus = t * extraJumpForce;

            float finalJumpForce = jumpForce + bonus;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

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
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
