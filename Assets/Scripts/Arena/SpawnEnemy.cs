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
    public ArenaController arena;
    public void Start()
    {
        spriteRenderer = enemy.GetComponentInChildren<SpriteRenderer>();
        transform.AddComponent<SpriteRenderer>();
        Instantiate(spriteRenderer, spriteParent.transform.position, Quaternion.identity, spriteParent.transform);
    }
    public void Spawn()
    {
        Enemy e = Instantiate(enemy, transform.position, Quaternion.identity).GetComponent<Enemy>();
        if(arena != null)
        {
            arena.currentEnemies.Add(e);
        }
        e.target = target;
        Destroy(this.gameObject);
    }
}
