using System;
using UnityEngine;

[Serializable]
public class ControllerParameters2D
{
    public enum JumpBehavior
    {
        ground,
        anywhere,
        disabled
    }

    [Range(0f, 90f)]
    public float SlopeLimit = 30f;

    public float GroundCheckHeight = 0.02f;
    public float SlopeCheckHeight = 0.5f;
    public float SlopeStickForceMultiplier = 0.2f;

    public float GravityScale;

    public Vector2 MaxVel = new Vector2(float.MaxValue, float.MaxValue);

    public JumpBehavior jumpBehavior;
    public float JumpReducer;
    public float JumpCooldown = 0.2f;

    public float DashVelocity;
    public float DashCooldown;
    public float TotalDashDuration;
}
