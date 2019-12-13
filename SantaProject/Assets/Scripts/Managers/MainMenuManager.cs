using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        StartCoroutine(coStartGame());
    }
   
    private IEnumerator coStartGame()
    {
        SceneFader.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(1);
    }
}
