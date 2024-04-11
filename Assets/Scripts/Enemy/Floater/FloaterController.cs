using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterController : MonoBehaviour
{
    public float speed;
    public Transform target;
    private Transform _transform;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _transform = transform;
    }

    void Update()
    {
        Physics2D.Raycast(_transform.position, target.position - _transform.position);
        Debug.DrawRay(_transform.position, target.position - _transform.position, Color.red);
    }
}
