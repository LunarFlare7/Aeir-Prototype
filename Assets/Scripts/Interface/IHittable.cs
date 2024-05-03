using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    public void Hit(float dmg, Vector2 direction = new Vector2(), float knockbackMult = 1);
}
