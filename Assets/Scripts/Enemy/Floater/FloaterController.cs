using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FloaterController : MonoBehaviour
{
    public Transform target;
    public AIDestinationSetter destinationSetter;
    public AIPath path;
    public float speed;
    private LayerMask selfMask;
    private Transform _transform;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    void Start()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        path = GetComponent<AIPath>();
        destinationSetter.target = target;
        rb = GetComponent<Rigidbody2D>();
        _transform = transform;
        circleCollider = GetComponent<CircleCollider2D>();
        selfMask = ~LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
    }

    void Update()
    {
        Vector3 left = circleCollider.bounds.min + (circleCollider.bounds.extents.y * Vector3.up);
        RaycastHit2D hit = Physics2D.Raycast(left, target.position - left, Mathf.Infinity, selfMask);
        Debug.DrawRay(left, target.position - left, Color.red);

        if (hit.transform == target)
        {
            Debug.Log(hit.transform + ", " + transform + ", equal");
            path.maxSpeed = 0;
        }
        else
        {
            path.maxSpeed = speed;
            Debug.Log(hit.transform + ", " + transform + ", not equal");
        }
    }
}
