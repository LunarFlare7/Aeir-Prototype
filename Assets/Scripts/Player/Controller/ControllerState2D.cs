using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerState2D
{
    public bool IsJumping { get; set; }
    public bool IsOnSlope { get; set; }
    public float NormalizedMoveDirection { get; set; }
    public float SlopeAngle { get; set; }

    public bool IsDashing { get; set; }

    public bool IsGrounded { get; set; }

    public void Reset()
    {
        IsGrounded =
            IsOnSlope = false;

        NormalizedMoveDirection = 0;

        SlopeAngle = 0f;
    }

    public override string ToString()
    {
        return string.Format(
            "controller: (grounded:{0} on-slope:{1} move-dir:{2} slope-angle:{3} jumping:{4} dashing:{5})",
            IsGrounded,
            IsOnSlope,
            NormalizedMoveDirection,
            SlopeAngle,
            IsJumping,
            IsDashing);
    }

}
