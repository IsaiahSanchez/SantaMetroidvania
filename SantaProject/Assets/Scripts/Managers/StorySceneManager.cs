using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StorySceneManager : MonoBehaviour
{
    public static StorySceneManager instance;

    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private float textFadeSpeed = 1f;
    [SerializeField] private float textWaitTime = 3f;
    [SerializeField] private List<string> listOfText = new List<string>();

    private Animator anim;

    private int currentLineOfText = 0;
    private bool doneFading = false;

    private void Start()
    {
        if (instance == null)
        {
            StorySceneManager.instance = this;
        }
        else
        {
            Destroy(this);
        }

        SceneFader.instance.FadeToClear();
        anim = storyText.gameObject.GetComponentInParent<Animator>();
        StartCoroutine(waitToStartStory());
    }


    private IEnumerator waitToStartStory()
    {
        yield return new WaitForSeconds(1.5f);
        startStory();
    }
    public void startStory()
    {
        storyText.text = listOfText[0];
        fadeInText();
    }

    
    private  void fadeInText()
    {
        anim.ResetTrigger("fadeIn");
        anim.SetTrigger("fadeIn");
        StartCoroutine(waitToFadeOut());
    }
    private IEnumerator waitToFadeOut()
    {
        yield return new WaitForSeconds(textWaitTime);
        fadeOutText();
    } 

    private void fadeOutText()
    {
        anim.ResetTrigger("fadeOut");
        anim.SetTrigger("fadeOut");
        currentLineOfText++;
        if (currentLineOfText < 3)
        {
            StartCoroutine(waitToFadeIn());
        }
        else
        {
            StartCoroutine(waitToGoToGame());
        }

    }
    private IEnumerator waitToFadeIn()
    {
        yield return new WaitForSeconds(2f);
        storyText.text = listOfText[currentLineOfText];
        fadeInText();
    }
    private IEnumerator waitToGoToGame()
    {
        yield return new WaitForSeconds(2f);
        SceneFader.instance.FadeToBlack();
        yield return new WaitForSeconds(1f);
        goToGame();
    }


    public void goToGame()
    {
        SceneManager.LoadScene(1);
    }
}
