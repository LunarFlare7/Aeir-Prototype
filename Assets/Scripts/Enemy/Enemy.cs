using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHittable
{
    [Header("Settings")]
    public float maxHealth;
    public float health;
    public float speed;
    public float knockbackMult;
    public bool invincible;
    public GameObject deathEffect;
    public ParticleSystem hitEffect;

    public ArenaController arena;

    [Header("Target")]
    public Transform target;

    public void Start()
    {
        health = maxHealth;
        target = GameManager.Instance.player.transform;
    }

    public void Hit(float dmg, Vector2 dir, float kbMult)
    {
        if(invincible)
        {
            return;
        }
        health -= dmg;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        hitEffect.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        hitEffect.Play();
        if (health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
