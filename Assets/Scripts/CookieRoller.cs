using UnityEngine;

public class Cookie : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float rollForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayerMask;

    private Rigidbody2D rb;
    private CircleCollider2D col;
    private bool hasStartedRolling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();

        if (rb == null)
        {
            Debug.LogError("Cookie: Rigidbody2D component not found!");
            return;
        }

        if (col == null)
        {
            Debug.LogError("Cookie: CircleCollider2D component not found!");
            return;
        }

        Invoke(nameof(StartRolling), 0.1f);
    }

    void StartRolling()
    {
        if (IsOnGround() && !hasStartedRolling)
        {
            rb.AddForce(Vector2.left * rollForce, ForceMode2D.Impulse);
            hasStartedRolling = true;
        }
        else if (!IsOnGround())
        {
            Invoke(nameof(StartRolling), 0.1f);
        }
    }

    bool IsOnGround()
    {
        Vector2 rayOrigin = transform.position;
        float rayDistance = col.radius + groundCheckDistance;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, groundLayerMask);

        return hit.collider != null;
    }

    void OnDrawGizmosSelected()
    {
        if (col != null)
        {
            Gizmos.color = IsOnGround() ? Color.green : Color.red;
            Vector3 rayStart = transform.position;
            Vector3 rayEnd = rayStart + Vector3.down * (col.radius + groundCheckDistance);
            Gizmos.DrawLine(rayStart, rayEnd);
        }
    }
}