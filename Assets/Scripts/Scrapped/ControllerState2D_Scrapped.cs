using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerState2D_Scrapped
{
    public bool IsCollidingRight { get; set; }
    public bool IsCollidingLeft { get; set; }
    public bool IsCollidingUp { get; set; }
    public bool IsCollidingDown { get; set; }
    public bool IsMovingUpSlope { get; set; }
    public bool IsMovingDownSlope { get; set; }
    public bool IsGrounded { get { return IsCollidingDown; } }
    public float SlopeAngle { get; set; }

    public bool IsColliding { get { return IsCollidingLeft || IsCollidingRight || IsCollidingUp || IsCollidingDown; } }

    public void Reset()
    {
        IsCollidingLeft =
            IsCollidingRight =
            IsCollidingUp =
            IsCollidingDown =
            IsMovingUpSlope =
            IsMovingDownSlope = false;

        SlopeAngle = 0;
    }

    public override string ToString()
    {
        return string.Format(
            "(controller: r:{0} l:{1} u:{2} d:{3} up-slope:{4} down-slope:{5} angle:{6}",
            IsCollidingRight,
            IsCollidingLeft,
            IsCollidingUp,
            IsCollidingDown,
            IsMovingUpSlope,
            IsMovingDownSlope,
            SlopeAngle);
    }
}
