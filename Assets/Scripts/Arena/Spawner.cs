using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Spawner
{
    public GameObject spawnObject;
    public List<Vector2> spawnPositions = new List<Vector2>();
}
