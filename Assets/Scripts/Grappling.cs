
using UnityEngine;

public class Grappling : MonoBehaviour
{
    public Transform rope;

    public float ropeWidth = 0.08f;

    // Grappling movement
    public float grappleSpeed = 20f;
    public float grappleAcceleration = 60f;

    private Rigidbody2D rb;

    private Collider2D grapplePoint;

    private bool isGrappling = false;
    public bool IsGrappling => isGrappling;
    private float currentGrappleSpeed = 0f;

    private Vector2 grappleDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rope.gameObject.SetActive(false);
    }

    void Update()
    {
        // Start grappling when clicking a GrapplePoint
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Find all colliders under the mouse
            Collider2D[] hits = Physics2D.OverlapPointAll(mousePosition);

            foreach (Collider2D hit in hits)
            {
                // Only attach to objects tagged GrapplePoint
                if (hit.CompareTag("GrapplePoint"))
                {
                    grapplePoint = hit;

                    grappleDirection =
                        (Vector2)grapplePoint.transform.position
                        - rb.position;

                    // Only grapple if not already at the point
                    if (grappleDirection.sqrMagnitude > 0.0001f)
                    {
                        grappleDirection.Normalize();

                        isGrappling = true;
                        currentGrappleSpeed = 0f;

                        rope.gameObject.SetActive(true);
                    }

                    break;
                }
            }
        }

        // Release grapple
        if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    void FixedUpdate()
    {
        if (isGrappling)
        {
            PullPlayer();
        }
    }

    void PullPlayer()
    {
        if (grapplePoint == null)
        {
            StopGrapple();
            return;
        }

        Vector2 direction =
            (Vector2)grapplePoint.transform.position - rb.position;

        // Disconnect once we've reached or passed the point
        if (Vector2.Dot(direction, grappleDirection) <= 0f)
        {
            StopGrapple();
            return;
        }

        // Accelerate toward the grapple point
        currentGrappleSpeed = Mathf.MoveTowards(
            currentGrappleSpeed,
            grappleSpeed,
            grappleAcceleration * Time.fixedDeltaTime
        );

        // Propel player toward the point
        rb.linearVelocity =
            direction.normalized * currentGrappleSpeed;
    }

    void StopGrapple()
    {
        isGrappling = false;
        currentGrappleSpeed = 0f;

        // Hide rope
        rope.gameObject.SetActive(false);

        grapplePoint = null;

        // Keep momentum after releasing
    }

    void LateUpdate()
    {
        if (isGrappling && grapplePoint != null)
        {
            DrawRope();
        }
    }

    void DrawRope()
    {
        Vector2 start = transform.position;
        Vector2 end = grapplePoint.transform.position;

        Vector2 direction = end - start;
        Vector2 middle = (start + end) / 2f;

        // Position rope
        rope.position = new Vector3(
            middle.x,
            middle.y,
            rope.position.z
        );

        // Rotate rope
        float angle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg;

        rope.rotation = Quaternion.Euler(0, 0, angle);

        // Stretch rope
        rope.localScale = new Vector3(
            direction.magnitude,
            ropeWidth,
            1f
        );
    }
}
