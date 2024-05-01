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
    public GameObject deathEffect;
    public float knockbackTime;
    private float knockbackTimer;
    public float knockbackMult;

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
            //Debug.Log("attack");
        }

        if (idle)
        {
            IdleMovement();
        }

        if (health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void IdleMovement()
    {
        transform.position += new Vector3(Mathf.Sin(Time.time * xSpeed) * Time.deltaTime * xDist, Mathf.Cos(Time.time * ySpeed) * Time.deltaTime * yDist, 0);
    }

    public void Hit(AttackManager atk)
    {
        health -= atk.damage;
        StartCoroutine(TakeKnockback(atk.dir * atk.knockbackModifier * this.knockbackMult));
    }

    private IEnumerator TakeKnockback(Vector2 dir)
    {
        knockbackTimer = 0;
        Vector2 startingDir = dir;
        while (knockbackTimer <= knockbackTime)
        {
            transform.position += new Vector3(dir.x * Time.deltaTime, dir.y * Time.deltaTime, 0);
            dir = Vector2.Lerp(startingDir, Vector2.zero, Mathf.Pow(knockbackTimer / knockbackTime, 2));
            knockbackTimer += Time.deltaTime;
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.isTrigger)
        {
            Debug.Log("stop");
            knockbackTimer = knockbackTime;
        }
    }
}