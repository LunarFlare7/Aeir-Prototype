using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{

    private float attackTimer;

    public float damage;
    public float attackTime;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<IHittable>() != null)
        {
            col.GetComponent<IHittable>().Hit(damage);
        }
    }

    void OnEnable()
    {
        attackTimer = attackTime;
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;

        if(attackTimer < 0)
        {
            gameObject.SetActive(false);
        }
    }
}
