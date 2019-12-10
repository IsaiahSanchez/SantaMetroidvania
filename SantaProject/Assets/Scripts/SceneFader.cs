using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;

    [SerializeField]private Image faderPanel;

    private float currentOpacity = 1f;
    private bool isDoneFading = false;
    private bool goPositive = false;

    [SerializeField]private float FadeTime = 1f;
    private float timeElapsed = 0f;

    private bool isSettingClickable = false;

    private void Awake()
    {
        MakeSingleton();
        FadeToClear();
    }

    private void MakeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            Object.DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isDoneFading == false)
        {
            handleFading();
            faderPanel.color = new Vector4(0, 0, 0, currentOpacity);    
        }

        if (currentOpacity <= 0)
        {
            if (isSettingClickable != true)
            {
                faderPanel.raycastTarget = false;
                //StartCoroutine(waitToSetClickable());
            }
        }
        else
        {
            faderPanel.raycastTarget = true;
        }
    }

    private IEnumerator waitToSetClickable()
    {
        isSettingClickable = true;
        yield return new WaitForSecondsRealtime(1f);
        faderPanel.raycastTarget = false;
        isSettingClickable = false;
    }

    private void handleFading()
    {
        currentOpacity += doMathForFade(Time.deltaTime, FadeTime);

        if (goPositive == true)
        {
            if (currentOpacity > .9f)
            {
                currentOpacity = 1f;
                isDoneFading = true;
            }
        }
        else
        {
            if (currentOpacity < .1f)
            {
                currentOpacity = 0;
                isDoneFading = true;
                Debug.Log(isDoneFading);
            }
        }
    }

    private float doMathForFade(float timePassed, float maxTime)
    {
        if (goPositive == true)
        {
            return timePassed / maxTime;
        }
        else
        {
            return -(timePassed / maxTime);
        }

    }


    public void FadeToBlack()
    {
        goPositive = true;
        isDoneFading = false;
    }

    public void FadeToClear()
    {

        goPositive = false;
        isDoneFading = false;
    }


    

}
