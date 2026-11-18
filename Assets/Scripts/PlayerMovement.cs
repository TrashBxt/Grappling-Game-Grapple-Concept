using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Grappling grappling;
    private float horizontal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grappling = GetComponent<Grappling>();
    }

    void Update()
    {
        // Get horizontal input
        horizontal = Input.GetAxisRaw("Horizontal");

        // Jump when grounded
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()
            && !grappling.IsGrappling)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );
        }
    }

    void FixedUpdate()
    {
        // Don't interfere with grappling
        if (grappling.IsGrappling)
            return;

        // Move left and right
        if (horizontal != 0)
        {
            rb.linearVelocity = new Vector2(
                horizontal * moveSpeed,
                rb.linearVelocity.y
            );
        }
        else if (IsGrounded())
        {
            // Stop horizontal movement on the ground
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            0.2f,
            groundLayer
        );
    }
}
