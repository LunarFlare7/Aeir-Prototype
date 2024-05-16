using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public GameObject spriteParent;
    public GameObject sprite;
    public Transform target;
    public ArenaController arena;
    public void Start()
    {
        if(enemy.transform.Find("Body").gameObject != null)
        {
            sprite = enemy.transform.Find("Body").gameObject;
            Instantiate(sprite, spriteParent.transform.position, Quaternion.identity, spriteParent.transform);
        }
    }
    public void Spawn()
    {
        Enemy e = Instantiate(enemy, transform.position, Quaternion.identity).GetComponent<Enemy>();
        if(arena != null)
        {
            arena.currentEnemies.Add(e);
        }
        e.arena = arena;
        e.target = target;
        Destroy(this.gameObject);
    }
}
