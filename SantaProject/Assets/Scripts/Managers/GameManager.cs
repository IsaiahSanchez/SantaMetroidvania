using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Camera CurrentMainCamera;
    public PlayerMain mainPlayer;
    public bool arachnaephobiaModeEnabled = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartGameOver();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartGameOver()
    {
        SceneManager.LoadScene(2);
    }

    public void StartWin()
    {
        SceneManager.LoadScene(3);
    }
}
