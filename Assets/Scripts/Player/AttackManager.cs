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
            col.GetComponent<IHittable>().Hit(this);
        }
    }

    void OnEnable()
    {
        if (dir.y != 0)
        {
            transform.localEulerAngles = new Vector3(0, 0, 90 * dir.y);
            dir.x = 0;
        } else if (dir.y == 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
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