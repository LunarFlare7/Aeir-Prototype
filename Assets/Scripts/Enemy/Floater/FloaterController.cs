using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FloaterController : MonoBehaviour, IHittable
{
    public Transform target;
    public AIDestinationSetter destinationSetter;
    public AIPath path;
    public bool attacking;
    private bool idle;
    //private LayerMask selfMask;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    [Header("Settings")]
    public float maxHealth;
    public float health;
    public float speed;
    public float attackRate;
    private float attackTimer;

    [Header("Idle")]
    public float xSpeed;
    public float ySpeed;
    public float xDist;
    public float yDist;

    void Start()
    {
        health = maxHealth;
        destinationSetter = GetComponent<AIDestinationSetter>();
        path = GetComponent<AIPath>();
        destinationSetter.target = target;
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        //selfMask = ~LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
    }

    void Update()
    {
        /*Vector3 left = circleCollider.bounds.min + (circleCollider.bounds.extents.y * Vector3.up);
        RaycastHit2D hitLeft = Physics2D.Raycast(left, target.position - left, Mathf.Infinity, selfMask);
        Debug.DrawRay(left, target.position - left, Color.red);

        Vector3 right = circleCollider.bounds.max - (circleCollider.bounds.extents.y * Vector3.up);
        RaycastHit2D hitRight = Physics2D.Raycast(right, target.position - right, Mathf.Infinity, selfMask);
        Debug.DrawRay(right, target.position - right, Color.red);*/
        if (path.reachedDestination)
        {
            attacking = true;
            idle = true;
        }
        else
        {
            idle = false;
        }

        if (attacking)
        {
            Debug.Log("attack");
        }

        if (idle)
        {
            IdleMovement();
        }
    }

    public void IdleMovement()
    {
        transform.position += new Vector3(Mathf.Sin(Time.time * xSpeed) * Time.deltaTime * xDist, Mathf.Cos(Time.time * ySpeed) * Time.deltaTime * yDist, 0);
        Debug.Log("idle");
    }

    public void Hit(float dmg)
    {
        health -= dmg;
    }
}
