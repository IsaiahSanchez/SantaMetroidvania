using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuManager : MonoBehaviour
{
    public void BackToMainMenu()
    {
        StartCoroutine(coBackToMainMenu());
    }
    private IEnumerator coBackToMainMenu()
    {
        SceneFader.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(0);
    } 

    public void RestartGame()
    {
        StartCoroutine(coRestartGame());
    }
    private IEnumerator coRestartGame()
    {
        SceneFader.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(1);
    }
}
