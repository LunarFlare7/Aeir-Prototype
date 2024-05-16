using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float minX;
    public float maxX;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        minX = arena.GetComponent<BoxCollider2D>().bounds.min.x;
        maxX = arena.GetComponent<BoxCollider2D>().bounds.max.x;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
