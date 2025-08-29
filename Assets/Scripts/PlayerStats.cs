using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("LAYERS")]
    public LayerMask groundLayer;

    [Header("JUMP SETTINGS")]
    public float jumpHeight = 15f;
    public float jumpCutMultiplier = 0.5f;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;

    [Header("FALL SETTINGS")]
    public float fallAcceleration = -40f;
    public float maxFallSpeed = -15f;

    [Header("MOVEMENT SETTINGS")]
    public float maxMoveSpeed = 8f;
    public float groundAcceleration = 60f;
    public float airAcceleration = 40f;
    public float groundDeceleration = 70f;
    public float airDeceleration = 50f;

    [Header("GROUND DETECTION")]
    public float grounderDistance = 0.05f;
    public float groundingForce = -0.5f;

    [Header("SLOPE HANDLING")]
    public float slopeCheckDistance = 0.3f;
    public float maxSlopeAngle = 45f;

    [Header("FRICTION MATERIALS")]
    public PhysicsMaterial2D groundedFriction;
    public PhysicsMaterial2D airFriction;
}
