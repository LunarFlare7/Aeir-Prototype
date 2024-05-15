using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{

    public GameObject vcam;

    public bool affectPlayer;

    public dashType setDash;

    public enum dashType
    {
        normal,
        infinite,
        disabled
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            vcam.gameObject.SetActive(true);
            CharacterController2D controller = other.GetComponent<CharacterController2D>();
            if (affectPlayer)
            {
                //change player values here
            }

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            vcam.gameObject.SetActive(false);
        }
    }
}
