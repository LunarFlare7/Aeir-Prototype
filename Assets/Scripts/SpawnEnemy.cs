using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public GameObject spriteParent;
    public SpriteRenderer spriteRenderer;
    public Transform target;
    public void Start()
    {
        spriteRenderer = enemy.GetComponentInChildren<SpriteRenderer>();
        transform.AddComponent<SpriteRenderer>();
        Instantiate(spriteRenderer, transform.position, Quaternion.identity, spriteParent.transform);
    }
    public void Spawn()
    {
        FloaterController fl = Instantiate(enemy, transform.position, Quaternion.identity).GetComponent<FloaterController>();
        fl.target = target;
        Destroy(this.gameObject);
    }
}
