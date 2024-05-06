using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIn : MonoBehaviour
{
    public float time;
    void OnEnable()
    {
        StartCoroutine(DestroyInTime(time));
    }

    IEnumerator DestroyInTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
