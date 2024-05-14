using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Item, IItem
{
    public float healthIncrease;

    public new void Collect()
    {
        GameManager.Instance.player.GetComponent<Player>().health += healthIncrease;
        base.Collect();
    }
}
