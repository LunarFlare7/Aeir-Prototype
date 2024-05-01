using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IHittable
{

    private CharacterController2D _controller;

    [Header("Move")]
    private bool _isFacingRight;
    public float maxSpeed;
    public float accelerationOnGround;
    public float accelerationInAir;
    public bool handleInput;

    [Header("Animation")]
    public Animator ani;

    [Header("Attack")]
    public AttackManager meleeAttack;
    public float attackSpeed;
    private float attackTimer;

    private Vector2 _moveDir;


    [Header("Jump")]
    public float jumpHeight;

    void Start()
    {
        _controller = GetComponent<CharacterController2D>();
        _isFacingRight = transform.localScale.x > 0;
    }

    void Update()
    {
        if (handleInput)
        {
            HandleInputs();
        } else
        {
            _moveDir = Vector2.zero;
        }
        HandleMovement();
        UpdateFacingDirection();
        UpdateAnimations();
        TimerVariables();
    }

    private void TimerVariables()
    {
        if(attackTimer < attackSpeed)
        {
            attackTimer += Time.deltaTime;
        }
    }

    private void UpdateAnimations()
    {
        if(_moveDir.x != 0f && _controller.State.IsGrounded)
        {
            ani.SetBool("walking", true);
        } else
        {
            ani.SetBool("walking", false);
        }
    }

    private void UpdateFacingDirection()
    {
        if (_controller.Velocity.x > 0)
        {
            _isFacingRight = true;
            //remove when animations are added
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if (_controller.Velocity.x < 0)
        {
            _isFacingRight = false;
            //remove when animations are added
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void HandleInputs()
    {
        _moveDir.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _controller.Jump(jumpHeight);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && _controller.State.IsJumping && !_controller.State.IsDashing)
        {
            _controller.CutJump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && (_moveDir.x != 0 || _moveDir.y != 0))
        {
            _controller.Dash(_moveDir);
        }

        if(Input.GetMouseButtonDown(0) && attackTimer >= attackSpeed)
        {
            Attack();
        }
    }

    private void HandleMovement()
    {
        var accelertionMultiplier = _controller.State.IsGrounded ? accelerationOnGround : accelerationInAir;
        if(_controller.State.SlopeAngle > _controller.Parameters.SlopeLimit)
        {
            _moveDir = Vector2.zero;
        }
        if (!_controller.State.IsDashing)
        {
            if (Mathf.Abs(_controller.MovementSpeed) <= maxSpeed * 1.05)
            {
                _controller.SetHorizontalSpeed(Mathf.MoveTowards(_controller.MovementSpeed, _moveDir.x * maxSpeed, Time.deltaTime * accelertionMultiplier));
            }

            if(_controller.Velocity.magnitude > maxSpeed * 1.05)
            {
                if((Mathf.Abs(_controller.Velocity.x) > _controller.Velocity.y) && Mathf.Sign(_moveDir.x * _controller.Velocity.x) == 1)
                {
                    _controller.SetDrag(1f);
                }
                else if (!_controller.State.IsJumping)
                {
                    _controller.SetDrag(3f);
                }
                
            }
            else
            {
                _controller.SetDrag(0f);
            }
        }

    }

    public void Attack()
    {
        meleeAttack.dir.y = _moveDir.y;
        meleeAttack.dir.x = _isFacingRight ? 1 : -1;
        meleeAttack.gameObject.SetActive(true);
        attackTimer = 0;
    }

    public void Hit(AttackManager atk)
    {

    }
}
