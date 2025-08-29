using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.U2D;
using UnityEngine.Rendering.Universal;
using UnityEditor.Rendering;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    public PlayerStats stats;

    private Rigidbody2D rb;
    private BoxCollider2D col;

    private float moveInput;
    private float coyoteCounter;
    private float jumpBufferCounter;

    private bool isGrounded;
    private bool isOnSlope;
    private Vector2 slopeNormal;

    private Coroutine flipCoroutine;

    // Hop settings
    [Header("HOP SETTINGS")]
    [SerializeField] private float hopForce = 12f;
    [SerializeField] private float hopForwardForce = 6f;
    [SerializeField] private float hopCooldown = 0.35f;
    private float hopTimer;

    public bool IsGrounded => isGrounded;

    private Vector3 baseScale;
    private int facingDirection = 1; //1 = right, -1 = left

    //Oxygen Loss Event
    private Coroutine oxygenCoroutine;

    private Light2D light2D;
    private SpriteRenderer sprite;

    [Header("Shield")]
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private float duration = 5f;
    private bool shieldActive = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        baseScale = transform.localScale;

        light2D = GetComponentInChildren<Light2D>();
        sprite = GetComponent<SpriteRenderer>();

        if (shieldObject != null)
            shieldObject.SetActive(false);
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0 && facingDirection != 1)
            SetFacingDirection(1);
        else if (moveInput < 0 && facingDirection != -1)
            SetFacingDirection(-1);

        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = stats.jumpBufferTime;

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * stats.jumpCutMultiplier);

        jumpBufferCounter -= Time.deltaTime;
        coyoteCounter -= Time.deltaTime;
        hopTimer -= Time.deltaTime;

        CheckGrounded();
        CheckSlope();

        if (isGrounded)
            coyoteCounter = stats.coyoteTime;

        if (jumpBufferCounter > 0 && coyoteCounter > 0)
        {
            Hop(moveInput);
            jumpBufferCounter = 0;
        }
        else if (isGrounded && hopTimer <= 0f && Mathf.Abs(moveInput) > 0.1f)
        {
            Hop(moveInput);
        }
    }

    void FixedUpdate()
    {
        HandleAirControl();
        HandleFalling();
        ApplyFriction();
    }

    void CheckGrounded()
    {
        Vector2 origin = new Vector2(col.bounds.center.x, col.bounds.min.y + 0.02f);
        Vector2 size = new Vector2(col.bounds.size.x - 0.02f, 0.05f);

        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, Vector2.down, stats.grounderDistance, stats.groundLayer);
        isGrounded = hit.collider != null;
    }

    void CheckSlope()
    {
        isOnSlope = false;
        slopeNormal = Vector2.up;

        RaycastHit2D slopeHit = Physics2D.Raycast(transform.position, Vector2.down, stats.slopeCheckDistance, stats.groundLayer);
        if (slopeHit)
        {
            float angle = Vector2.Angle(slopeHit.normal, Vector2.up);
            if (angle > 0 && angle <= stats.maxSlopeAngle)
            {
                isOnSlope = true;
                slopeNormal = slopeHit.normal;
            }
        }
    }

    void Hop(float direction)
    {
        hopTimer = hopCooldown;
        coyoteCounter = 0;

        // Reset vertical velocity for consistent hops
        rb.velocity = new Vector2(rb.velocity.x, 0f);

        Vector2 hopDir;
        if (isOnSlope)
        {
            // Hop along the slope direction
            Vector2 slopeDir = Vector2.Perpendicular(slopeNormal).normalized * Mathf.Sign(direction);
            hopDir = slopeDir.normalized * hopForwardForce + Vector2.up * hopForce;
        }
        else
        {
            // Normal hop
            hopDir = new Vector2(direction * hopForwardForce, hopForce);
        }

        rb.AddForce(hopDir, ForceMode2D.Impulse);

        // Squash/stretch effect
        StartCoroutine(SquashStretch());
    }

    void HandleFalling()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * stats.fallAcceleration * Time.fixedDeltaTime;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, stats.maxFallSpeed));
        }
    }

    void ApplyFriction()
    {
        col.sharedMaterial = isGrounded ? stats.groundedFriction : stats.airFriction;
    }

    IEnumerator SquashStretch()
    {
        // Squash before hop
        transform.localScale = new Vector3(baseScale.x * 1.2f * facingDirection, baseScale.y * 0.8f, baseScale.z);
        yield return new WaitForSeconds(0.1f);

        // Stretch in air
        transform.localScale = new Vector3(baseScale.x * 0.8f * facingDirection, baseScale.y * 1.2f, baseScale.z);
        yield return new WaitUntil(() => isGrounded);

        // Return to base scale with correct facing
        transform.localScale = new Vector3(baseScale.x * facingDirection, baseScale.y, baseScale.z);
    }

    void HandleAirControl()
    {
        if (!isGrounded && Mathf.Abs(moveInput) > 0.1f)
        {
            float targetSpeed = moveInput * stats.maxMoveSpeed;
            float speedDif = targetSpeed - rb.velocity.x;
            float accel = stats.airAcceleration;

            float movement = speedDif * accel * Time.fixedDeltaTime;

            rb.velocity = new Vector2(
                Mathf.Clamp(rb.velocity.x + movement, -stats.maxMoveSpeed, stats.maxMoveSpeed),
                rb.velocity.y
            );
        }
    }

    void SetFacingDirection(int newDirection)
    {
        facingDirection = newDirection;

        if (flipCoroutine != null)
            StopCoroutine(flipCoroutine);

        flipCoroutine = StartCoroutine(FlipTween(newDirection));
    }

    IEnumerator FlipTween(int dir)
    {
        float duration = 0.1f;
        float t = 0f;

        Vector3 startScale = transform.localScale;
        Vector3 targetScale = new Vector3(baseScale.x * dir, baseScale.y, baseScale.z);

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, progress);
            yield return null;
        }

        transform.localScale = targetScale;
    }

    //Oxygen Loss Event
    public void StartOxygenLoss(float duration)
    {
        if (oxygenCoroutine != null)
            StopCoroutine(oxygenCoroutine);

        oxygenCoroutine = StartCoroutine(OxygenRoutine(duration));
    }

    private IEnumerator OxygenRoutine(float duration)
    {
        float elapsed = 0f;
        Color startLight = light2D != null ? light2D.color : Color.white;
        Color endLight = Color.blue;

        Color startSprite = sprite != null ? sprite.color : Color.white;
        Color endSprite = Color.blue;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (light2D != null)
                light2D.color = Color.Lerp(startLight, endLight, t);

            if (sprite != null)
                sprite.color = Color.Lerp(startSprite, endSprite, t);

            yield return null;
        }

        yield return FindObjectOfType<OxygenLossHazard>().DeathFallRoutine(this);
    }
    public void CancelOxygenLoss()
    {
        if (oxygenCoroutine != null)
        {
            StopCoroutine(oxygenCoroutine);
            oxygenCoroutine = null;
        }

        if (light2D != null) light2D.color = Color.white;
        if (sprite != null) sprite.color = Color.white;
    }

    //Shield
    public void ActivateShield(float duration)
    {
        if (shieldObject == null) return;

        if (shieldActive) return;

        shieldActive = true;
        shieldObject.SetActive(true);
        Invoke(nameof(DeactivateShield), duration);
    }

    private void DeactivateShield()
    {
        shieldActive = false;
        if (shieldObject != null)
            shieldObject.SetActive(false);
    }

    public bool IsShieldActive()
    {
        return shieldActive;
    }
}

