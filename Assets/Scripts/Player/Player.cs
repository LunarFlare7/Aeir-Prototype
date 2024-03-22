using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

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

        var accelertionMultiplier = _controller.State.IsGrounded ? accelerationOnGround : accelerationInAir;

        _curveScanner = Mathf.MoveTowards(_curveScanner, Input.GetAxisRaw("Horizontal"), Time.deltaTime * accelertionMultiplier);
        _curveScanner = Mathf.Clamp(_curveScanner, accelerationCurve.keys[0].time, accelerationCurve.keys[accelerationCurve.length - 1].time);
        //accelerationCurve.Evaluate(_curveScanner) * maxSpeed
        //Mathf.Lerp(_controller.MovementSpeed, Input.GetAxisRaw("Horizontal") * maxSpeed, Time.deltaTime * accelertionMultiplier)
        _controller.SetHorizontalSpeed(accelerationCurve.Evaluate(_curveScanner) * maxSpeed);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _controller.Jump(jumpHeight);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && _controller.State.IsJumping)
        {
            _controller.CutJump();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            _controller.Dash(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        }
    }
}
