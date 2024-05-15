using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour
{
    public GameObject restartText;
    public Transform player;
    public CinemachineVirtualCamera vCam;
    private bool restart;

    private static GameManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }
    public void Restart()
    {
        restartText.SetActive(true);
        restart = true;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && restart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
