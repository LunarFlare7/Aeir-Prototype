using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float minX;
    public float maxX;
    public Animator ani;
    public float spaceFromEdge;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        minX = arena.GetComponent<BoxCollider2D>().bounds.min.x;
        maxX = arena.GetComponent<BoxCollider2D>().bounds.max.x;
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && other.GetComponent<IHittable>() != null)
        {
            Vector2 dir = (other.transform.position - transform.position).normalized * Vector2.right;
            other.GetComponent<IHittable>().Hit(1, dir);
        }
    }
}
