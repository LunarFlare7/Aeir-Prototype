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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(((1 << collision.gameObject.layer) & ground) != 0)
        {
            rb.isKinematic = true;
            transform.position = collision.GetContact(0).point;
        }
    }
}
