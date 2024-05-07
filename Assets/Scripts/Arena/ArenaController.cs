using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ArenaController : MonoBehaviour
{
    public Transform target;
    public GameObject door;

    public List<Wave> waves;

    public List<Enemy> currentEnemies;

    public int currentWave;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            currentWave = 0;
            door.SetActive(true);
            StartCoroutine(NextWave());
        }
    }

    public IEnumerator NextWave()
    {
        yield return new WaitForSeconds(waves[currentWave].delay);
        SpawnWave();
    }

    public void SpawnWave()
    {
        List<SpawnEnemy> spawnObjects = new List<SpawnEnemy>();
        for (int i = 0; i < waves[currentWave].spawners.Count; i++)
        {
            for (int j = 0; j < waves[currentWave].spawners[i].spawnPositions.Count; j++)
            {
                SpawnEnemy spawnObject = Instantiate(waves[currentWave].spawners[i].spawnObject, waves[currentWave].spawners[i].spawnPositions[j], Quaternion.identity).GetComponent<SpawnEnemy>();
                spawnObject.target = target;
                spawnObject.arena = this;
                spawnObjects.Add(spawnObject);
            }
        }
        StartCoroutine(CheckWave(spawnObjects));
    }

    public IEnumerator CheckWave(List<SpawnEnemy> spawnObjects)
    {
        yield return new WaitUntil(() => spawnObjects.TrueForAll(obj => obj == null));
        yield return new WaitUntil(() => currentEnemies.TrueForAll(obj => obj == null));
        currentWave++;
        if(currentWave < waves.Count)
        {
            StartCoroutine(NextWave());
        }
        else
        {
             Destroy(door);
        }
    }
}
