using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject restartText;
    private bool restart;
    public void Restart()
    {
        restartText.SetActive(true);
        restart = true;
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && restart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
