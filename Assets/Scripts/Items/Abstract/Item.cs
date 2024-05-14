using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    public void Collect()
    {
        Destroy(gameObject);
    }
}
