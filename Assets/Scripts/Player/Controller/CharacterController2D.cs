using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{

    public ControllerParameters2D defaultParams;

    public LayerMask GroundMask;
    private Rigidbody2D rb;
    public ControllerState2D State { get; private set; }
    public ControllerParameters2D Parameters { get { return _overrideParameters ?? defaultParams; } }

    public Vector2 Velocity { get { return rb.velocity; } }
    public float MovementSpeed { get { return _movementSpeed; } }

    [SerializeField]
    private float _movementSpeed;

    private bool _canDash
    {
        get
        {
            if (dashes > 0)
            {
                return true;
            }
            return false;
        }
    }
    private float _dashTimer;
    private float _dashDuration;
    private Vector2 _dashDir;
    [SerializeField]
    private float dashes;

    private bool _canJump
    {
        get
        {
            if (_jumpTimer >= Parameters.JumpCooldown)
            {
                if (Parameters.jumpBehavior == ControllerParameters2D.JumpBehavior.ground && State.IsGrounded)
                {
                    return true;
                }
                else if (Parameters.jumpBehavior == ControllerParameters2D.JumpBehavior.anywhere)
                {
                    return true;
                }
            }
            return false;
        }
    }
    private float _jumpTimer;

    private Vector2 _slopeNormal;
    private Vector2 _slopePerpendicular;
    private bool _awayFromSlope;

    private Transform _transform;
    private BoxCollider2D _collider;
    private Vector2 _groundCheckPos;
    private ControllerParameters2D _overrideParameters;

    void Awake()
    {
        State = new ControllerState2D();
        rb = GetComponent<Rigidbody2D>();
        _transform = transform;
        _collider = GetComponent<BoxCollider2D>();
    }

    void LateUpdate()
    {
        //Always First
        UpdateState();

        UpdateMovementSpeed();

        UpdateTimerVariables();

        if(State.IsDashing)
        {
            DashUpdate();
            _dashDuration += Time.deltaTime;
        }
    }

    public void SetHorizontalSpeed(float x)
    {
        Move(new Vector2(x, rb.velocity.y));
    }

    public void SetVerticalSpeed(float y)
    {
        Move(new Vector2(rb.velocity.x, y));
    }

    public void Move(Vector2 deltaMovement)
    {
        HandleSlopes(ref deltaMovement);
        rb.velocity = deltaMovement;
    }

    public void Jump(float jumpHeight)
    {
        if (_canJump)
        {
            State.IsJumping = true;
            float jumpVelocity = Mathf.Sqrt(-2 * jumpHeight * (Physics2D.gravity.y * Parameters.RisingGravityScale));
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
            _jumpTimer = 0;
        }
    }

    public void CutJump()
    {
        SetVerticalSpeed(rb.velocity.y / Parameters.JumpReducer);
    }

    public void Dash(Vector2 dashDir)
    {
        if (_canDash)
        {
            _dashDir = dashDir;
            _dashDir.Normalize();
            State.IsDashing = true;
            State.IsJumping = false;
            rb.velocity = _dashDir * Parameters.DashVelocity;
            SetDrag(Parameters.DashDrag);
            dashes -= 1;
            //Debug.DrawRay(_transform.position, _dashDir, Color.red, Mathf.Infinity);
        }
    }

    private void DashUpdate()
    {
        SetDrag(rb.drag - Parameters.DashDrag/Parameters.TotalDashDuration * Time.deltaTime);
        if (_dashDuration >= Parameters.TotalDashDuration)
        {
            EndDash();
        }
    }

    private void EndDash()
    {
        State.IsDashing = false;
        _dashTimer = 0;
        _dashDuration = 0;
        //Debug.DrawRay(_transform.position, -_dashDir, Color.blue, Mathf.Infinity);
        SetDrag(0f);
    }

    private void HandleSlopes(ref Vector2 deltaMovement)
    {
        if (State.IsOnSlope && !State.IsJumping)
        {
            Vector2 newPerpendicular = Mathf.Sign(deltaMovement.x) * -_slopePerpendicular;
            if (State.SlopeAngle > Parameters.SlopeLimit)
            {
                deltaMovement = rb.velocity;
                return;
            }
            Debug.DrawRay(_groundCheckPos, newPerpendicular, Color.blue);
            Debug.DrawRay(_groundCheckPos, Vector2.Reflect(newPerpendicular, _slopePerpendicular), Color.blue);
            deltaMovement.Set(Mathf.Abs(deltaMovement.x) * newPerpendicular.x, Mathf.Abs(deltaMovement.x) * newPerpendicular.y);
        }
    }

    private void UpdateState()
    {
        State.Reset();
        _groundCheckPos.Set(_transform.position.x, _collider.bounds.min.y);
        GroundCheck();
        SlopeCheck();

        DashCountUpdate();

        DecideGravityScale();

        if (rb.velocity.y <= 0.01f)
        {
            State.IsJumping = false;
        }

        if (Mathf.Abs(rb.velocity.x) > 0.001f)
        {
            State.NormalizedMoveDirection = Mathf.Sign(rb.velocity.x);
        }
    }

    private void DashCountUpdate()
    {
        if (_dashTimer >= Parameters.DashCooldown && !State.IsDashing)
        {
            if (Parameters.dashBehavior == ControllerParameters2D.DashRefillBehavior.ground && State.IsGrounded)
            {
                dashes = Parameters.DashAmount;
            }
            if (Parameters.dashBehavior == ControllerParameters2D.DashRefillBehavior.infinite)
            {
                dashes = Parameters.DashAmount;
            }
        }
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_groundCheckPos, new Vector2(_collider.size.x, Parameters.GroundCheckHeight), 0, Vector2.down, Parameters.GroundCheckHeight, GroundMask);

        if (hit)
        {
            State.IsGrounded = true;
        }
    }

    private void SlopeCheck()
    {
        RaycastHit2D slopeCheckHit = Physics2D.BoxCast(_groundCheckPos, new Vector2(_collider.size.x - 0.1f, Parameters.GroundCheckHeight), 0, Vector2.down, Parameters.SlopeCheckHeight, GroundMask) ;
        if (slopeCheckHit && (State.IsGrounded || !_awayFromSlope))
        {
            State.IsGrounded = true;
            _slopeNormal = slopeCheckHit.normal;
            _slopePerpendicular = Vector2.Perpendicular(_slopeNormal).normalized;
            State.SlopeAngle = Vector2.Angle(_slopeNormal, Vector2.up);

            if (Math.Abs(_slopeNormal.x) > 0.1f)
            {
                State.IsOnSlope = true;       
                _awayFromSlope = false;
            }
        }
        else
        {
            _awayFromSlope = true;
        }

        if (State.SlopeAngle > Parameters.SlopeLimit)
        {
            State.IsGrounded = false;
        }
    }

    private void DecideGravityScale()
    {

        if (State.IsOnSlope)
        {
            rb.gravityScale = Parameters.RisingGravityScale / 3;
            if (State.SlopeAngle > Parameters.SlopeLimit)
            {
                rb.gravityScale = Parameters.RisingGravityScale;
            }
        }
        else if (State.IsGrounded || State.IsDashing)
        {
            rb.gravityScale = 0f;
        }
        else
        {
            if(rb.velocity.y > 0f)
            {
                rb.gravityScale = Parameters.RisingGravityScale;
            } else if (rb.velocity.y < 0f)
            {
                rb.gravityScale = Parameters.FallingGravityScale;
            } else
            {
                rb.gravityScale = Parameters.RisingGravityScale;
            }
        }
    }

    private void UpdateMovementSpeed()
    {
        if(State.IsOnSlope && !State.IsJumping)
        {
            _movementSpeed = rb.velocity.magnitude * Mathf.Sign(rb.velocity.x);
        } else
        {
            _movementSpeed = rb.velocity.x;
        }
    }

    private void UpdateTimerVariables()
    {
        if (_dashTimer <= Parameters.DashCooldown)
        {
            _dashTimer += Time.deltaTime;
        }
        if(_jumpTimer <= Parameters.JumpCooldown)
        {
            _jumpTimer += Time.deltaTime;
        }
    }

    public void SetDrag(float drag)
    {
        rb.drag = drag;
    }

    public void Freeze()
    {
        rb.isKinematic = true;
    }
}
