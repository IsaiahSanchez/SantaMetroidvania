using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CinemachineVirtualCamera MainCamera;
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
        StartCoroutine(coStartGameOver());
    }
    private IEnumerator coStartGameOver()
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneFader.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(2);
    }

    public void StartWin()
    {
        StartCoroutine(coStartWin());
    }
    private IEnumerator coStartWin()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneFader.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(3);
    }
}
