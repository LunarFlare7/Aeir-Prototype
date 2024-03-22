using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform player;

    public float panSpeed;
    public float maxDistance;
    public Vector3 offset;

    private void Start()
    {
        transform.position = player.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destination = player.position + offset;
        float speed = panSpeed + Vector3.Distance(transform.position, destination);
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * panSpeed);
    }
}
