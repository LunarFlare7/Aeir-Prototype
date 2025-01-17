using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{

    public LayerMask ground;
    public Rigidbody2D rb;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = (Vector3)rb.velocity.normalized;

        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, target));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(((1 << col.gameObject.layer) & ground) != 0)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            transform.position = transform.position - new Vector3(0, 0.225f, 0);
        }

        if(col.GetComponent<IHittable>() != null)
        {
            Vector2 dir = (col.transform.position - transform.position).normalized * Vector2.right;
            col.GetComponent<IHittable>().Hit(1, dir);
        }
    }
}
