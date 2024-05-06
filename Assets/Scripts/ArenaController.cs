using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ArenaController : MonoBehaviour
{
    public Transform target;

    public List<Vector3> locations = new List<Vector3>();
    public List<GameObject> enemies = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            for (int i = 0; i < locations.Count; i++)
            {
                Instantiate(enemies[(int)locations[i].z], transform.position + locations[i], Quaternion.identity);
            }
        }
    }
}
