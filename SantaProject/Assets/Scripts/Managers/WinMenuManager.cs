using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WinMenuManager : MonoBehaviour
{
    public static WinMenuManager instance;

    [SerializeField] private TextMeshProUGUI presentCount;


    private void Awake()
    {
        instance = this;
    }

    public void Restart()
    {
        StartCoroutine(coRestart());
    }
    private IEnumerator coRestart()
    {
        SceneFader.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        StartCoroutine(coMainMenu());
    }
    private IEnumerator coMainMenu()
    {
        SceneFader.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(0);
    }

    public void setPresentsGotten(int count)
    {
        presentCount.text = count.ToString();
    }


    public void Exitgame()
    {
        Application.Quit();
    }
}
