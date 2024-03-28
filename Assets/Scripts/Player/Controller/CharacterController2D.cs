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

    private bool _canDash;
    private float _dashTimer;
    private float _dashDuration;
    private Vector2 _dashDir;
    [SerializeField]
    private float _dragAmount;

    private bool _canJump
    {
        get
        {
            if (Parameters.jumpBehavior == ControllerParameters2D.JumpBehavior.ground && State.IsGrounded)
            {
                return true;
            } else if (Parameters.jumpBehavior == ControllerParameters2D.JumpBehavior.anywhere && _jumpTimer >= Parameters.JumpCooldown)
            {
                return true;
            }
            return false;
        }
    }
    private float _jumpTimer;

    private Vector2 _slopeNormal;
    private Vector2 _slopePerpendicular;
    private bool _awayFromSlope;

    private Transform _transform;
    private Vector3 _localScale;
    private BoxCollider2D _collider;
    private Vector2 _groundCheckPos;
    private ControllerParameters2D _overrideParameters;

    void Awake()
    {
        State = new ControllerState2D();

        rb = GetComponent<Rigidbody2D>();
        _transform = transform;
        _localScale = transform.localScale;
        _collider = GetComponent<BoxCollider2D>();
        rb.gravityScale = Parameters.GravityScale;
    }

    void LateUpdate()
    {
        //Always First
        UpdateState();

        UpdateMovementSpeed();

        UpdateTimerVariables();

        if (State.IsGrounded && _dashTimer >= Parameters.DashCooldown && !State.IsDashing)
        {
            _canDash = true;
        }

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
        //_movementSpeed = deltaMovement.x;
        HandleSlopes(ref deltaMovement);
        rb.velocity = deltaMovement;
    }

    public void Jump(float jumpHeight)
    {
        if (_canJump)
        {
            State.IsJumping = true;
            float jumpVelocity = Mathf.Sqrt(-2 * jumpHeight * (Physics2D.gravity.y * Parameters.GravityScale));
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
            _canDash = false;
            Move(_dashDir * Parameters.DashVelocity);
            SetDrag(_dragAmount);
        }
    }

    private void DashUpdate()
    {
        SetDrag(rb.drag - _dragAmount/Parameters.TotalDashDuration * Time.deltaTime);
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
        SetDrag(0f);
    }

    private void HandleSlopes(ref Vector2 deltaMovement)
    {
        if (State.IsOnSlope && !State.IsJumping)
        {
            Vector2 newPerpendicular = Mathf.Sign(deltaMovement.x) * -_slopePerpendicular;
            if (!State.IsGrounded)
            {
                newPerpendicular = ((Mathf.Sign(deltaMovement.x) * -_slopePerpendicular) + (-_slopeNormal * Parameters.SlopeStickForceMultiplier)).normalized;
            }

            Debug.DrawRay(_groundCheckPos, newPerpendicular, Color.red);
            Debug.DrawRay(_groundCheckPos, Vector2.Reflect(newPerpendicular, _slopePerpendicular), Color.red);
            deltaMovement.Set(Mathf.Abs(deltaMovement.x) * newPerpendicular.x, Mathf.Abs(deltaMovement.x) * newPerpendicular.y);
        }
    }

    private void UpdateState()
    {
        State.Reset();
        _groundCheckPos.Set(_transform.position.x, _collider.bounds.min.y);
        GroundCheck();
        SlopeCheck();

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
        RaycastHit2D slopeCheckHit = Physics2D.BoxCast(_groundCheckPos, new Vector2(_collider.size.x, Parameters.GroundCheckHeight), 0, Vector2.down, Parameters.SlopeCheckHeight, GroundMask);
        if (slopeCheckHit && (State.IsGrounded || !_awayFromSlope))
        {
            State.IsGrounded = true;
            _slopeNormal = slopeCheckHit.normal;
            _slopePerpendicular = Vector2.Perpendicular(_slopeNormal).normalized;
            State.SlopeAngle = Vector2.Angle(_slopeNormal, Vector2.up);

            if (Math.Abs(_slopeNormal.x) > 0.01f)
            {
                State.IsOnSlope = true;
                if (State.IsGrounded)
                {
                    _awayFromSlope = false;
                }
            }
        }
        else
        {
            _awayFromSlope = true;
        }
    }

    private void DecideGravityScale()
    {
        if (State.IsOnSlope)
        {
            rb.gravityScale = Parameters.GravityScale / 3;
        }
        else if (State.IsGrounded || State.IsDashing)
        {
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = Parameters.GravityScale;
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
}
