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

    public enum DashBehavior
    {
        ground,
        anywhere,
        disabled
    }

    public float GroundCheckHeight = 0.02f;
    public float GravityScale;
    public Vector2 MaxVel = new Vector2(float.MaxValue, float.MaxValue);

    [Header("Slopes")]
    [Range(0f, 90f)]
    public float SlopeLimit = 30f;
    public float SlopeCheckHeight = 0.1f;

    [Header("Jump")]
    public JumpBehavior jumpBehavior;
    public float JumpReducer;
    public float JumpCooldown = 0.2f;

    [Header("Dash")]
    public DashBehavior dashBehavior;
    public float DashVelocity;
    public float DashCooldown;
    public float TotalDashDuration;
    
}
