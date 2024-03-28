using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{

    private CharacterController2D _controller;

    [Header("Move")]
    private bool _isFacingRight;
    public float maxSpeed;
    public float accelerationOnGround;
    public float accelerationInAir;
    public AnimationCurve accelerationCurve;
    private float _curveScanner;

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
        HandleInputs();
        UpdateFacingDirection();
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

        HandleMoveInputs();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _controller.Jump(jumpHeight);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && _controller.State.IsJumping)
        {
            _controller.CutJump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && (_moveDir.x != 0 || _moveDir.y != 0))
        {
            _curveScanner = 0;
            _controller.Dash(_moveDir);
        }
    }

    private void HandleMoveInputs()
    {
        var accelertionMultiplier = _controller.State.IsGrounded ? accelerationOnGround : accelerationInAir;
        if (!_controller.State.IsDashing)
        {
            if (Mathf.Abs(_controller.MovementSpeed) <= maxSpeed * 1.05)
            {
                _curveScanner = Mathf.MoveTowards(_curveScanner, _moveDir.x, Time.deltaTime * accelertionMultiplier);
                _controller.SetHorizontalSpeed(accelerationCurve.Evaluate(_curveScanner) * maxSpeed);
            }

            if(Mathf.Abs(_controller.MovementSpeed) > maxSpeed * 1.05)
            {
                _curveScanner = _controller.MovementSpeed/maxSpeed;
                if(Mathf.Sign(_moveDir.x * _controller.MovementSpeed) == 1)
                {
                    _controller.SetDrag(1f);
                }
                else
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
}
