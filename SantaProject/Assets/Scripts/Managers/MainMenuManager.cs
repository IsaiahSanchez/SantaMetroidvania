using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button Start;

    private void Update()
    {
        if (GameDataManager.instance.doesFileExist())
        {
            deleteButton.interactable = true;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(Start.gameObject);
            deleteButton.interactable = false;
        }
    }

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
