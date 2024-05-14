using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class FloaterController : Enemy, IHittable
{
    public AIDestinationSetter destinationSetter;
    public AIPath path;
    public bool preparingAttack;
    public bool attacking;
    public Animator ani;

    [Header("Settings")]
    public ParticleSystem hitEffect;
    public float knockbackTime;
    private float knockbackTimer;
    public GameObject projectile;
    public float projectileSpeed;
    private float attackRateTimer;
    public float aggroRange;

    [Header("Idle")]
    public float xSpeed;
    public float ySpeed;
    public float xDist;
    public float yDist;

    new void Start()
    {
        base.Start();
        destinationSetter = GetComponent<AIDestinationSetter>();
        path = GetComponent<AIPath>();
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        /*Vector3 left = circleCollider.bounds.min + (circleCollider.bounds.extents.y * Vector3.up);
        RaycastHit2D hitLeft = Physics2D.Raycast(left, target.position - left, Mathf.Infinity, selfMask);
        Debug.DrawRay(left, target.position - left, Color.red);

        Vector3 right = circleCollider.bounds.max - (circleCollider.bounds.extents.y * Vector3.up);
        RaycastHit2D hitRight = Physics2D.Raycast(right, target.position - right, Mathf.Infinity, selfMask);
        Debug.DrawRay(right, target.position - right, Color.red);*/

        if((transform.position - target.transform.position).magnitude < aggroRange)
        {
            destinationSetter.target = target;
        } else
        {
            destinationSetter.target = null;
        }

        if ((path.reachedDestination || destinationSetter.target == null) && !preparingAttack)
        {
            if(destinationSetter.target != null)
            {
                preparingAttack = true;
            }      
            ani.SetBool("Idle", true);
        }
        else
        {
            preparingAttack = false;
            ani.SetBool("Idle", false);
        }

        if (preparingAttack && !attacking)
        {
            attackRateTimer += Time.deltaTime;
            if (attackRateTimer >= attackRate)
            {
                attackRateTimer = 0;
                ani.SetTrigger("Attack");
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

    public void Attack()
    {
        GameObject projAttack = Instantiate(projectile, transform.position, Quaternion.FromToRotation(Vector3.right, target.position - transform.position));
        projAttack.GetComponent<Rigidbody2D>().velocity = ((Vector2)(target.position - transform.position) + Vector2.up).normalized * projectileSpeed;
        attackRateTimer = 0;
    }

    public void IdleMovement()
    {
        transform.position += new Vector3(Mathf.Sin(Time.time * xSpeed) * Time.deltaTime * xDist, Mathf.Cos(Time.time * ySpeed) * Time.deltaTime * yDist, 0);
    }

    public new void Hit(float dmg, Vector2 dir, float knockbackMult)
    {
        base.Hit(dmg, dir, knockbackMult);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        hitEffect.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        hitEffect.Play();
        invincible = true;
        StartCoroutine(TakeKnockback(dir * knockbackMult * this.knockbackMult));
        if (health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
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
        invincible = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.isTrigger)
        {
            knockbackTimer = knockbackTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger && other.GetComponent<IHittable>() != null)
        {
            Vector2 dir = (other.transform.position - transform.position).normalized * Vector2.right;
            other.GetComponent<IHittable>().Hit(1, dir);
        }
    }
}
