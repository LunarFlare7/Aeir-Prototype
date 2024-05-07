using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    public List<Spawner> spawners = new List<Spawner>();
    public float delay;
}
