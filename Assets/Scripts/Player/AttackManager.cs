using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackManager : MonoBehaviour
{

    private float attackTimer;

    public float damage;
    public float attackTime;
    public Vector2 dir;
    public float knockbackModifier;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<IHittable>() != null)
        {
            col.GetComponent<IHittable>().Hit(damage, dir, knockbackModifier);
        }
    }

    void OnEnable()
    {
        if (dir.y != 0)
        {
            dir.x = 0;
        }
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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