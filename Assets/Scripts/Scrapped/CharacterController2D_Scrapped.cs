using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterController2D_Scrapped : MonoBehaviour
{
    private const float _skinWidth = 0.2f;

    /*[SerializeField]
    [Range(0f, 20f)]
    private int _totalHorizontalRays = 8;
    [SerializeField]
    [Range(0f, 20f)]
    private int _totalVerticalRays = 4;*/

    private static readonly float s_slopeLimitTangent = Mathf.Tan(75f * Mathf.Deg2Rad);

    public LayerMask ObjectMask;
    public ControllerParameters2D DefaultParams;

    //public ControllerState2D State { get; private set; }
    public Vector2 Velocity { get { return _velocity; }}
    public bool CanJump {  get; private set; }

    private Vector2 _velocity;
    private Transform _transform;
    private Vector3 _localScale;
    private BoxCollider2D _collider;

    private float
        _verticalDistanceBetweenRays,
        _horizontalDistanceBetweenRays;

    public void Awake()
    {
        //State = new ControllerState2D();
        _transform = transform;
        _localScale = transform.localScale;
        _collider = GetComponent<BoxCollider2D>();
    }

    public void AddForce(Vector2 force)
    {
        _velocity += force;
    }

    public void SetForce(Vector2 force)
    {
        _velocity = force;
    }

    public void SetHorizontalForce(float x)
    {
        _velocity.x = x;
    }

    public void SetVerticalForce(float y)
    {
        _velocity.y = y;
    }

    public void LateUpdate()
    {
        Debug.DrawRay(_transform.position, Vector2.up * 10f, Color.red);
        var raycastHit2 = Physics2D.Raycast(_transform.position, Vector2.up, 10f, ObjectMask);
        Debug.Log("(" + raycastHit2.collider +")");

    }

    private void Move(Vector2 deltaMovement)
    {

    }

    /*private void HandleOneWayPlatforms()
    {

    }*/

    private void CalculateRayOrigins()
    {

    }

    private void MoveHorizontally(ref Vector2 deltaMovement)
    {

    }

    private void MoveVertically(ref Vector2 deltaMovement)
    {

    }

    private void HandleVerticalSlope(ref Vector2 deltaMovement)
    {

    }

    private void HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {

    }
}