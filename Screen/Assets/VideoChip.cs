using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoChip : MonoBehaviour
{
    private float refreshSpeed = .01f;
    private WaitForSeconds wait;

    private int ptrX = 0, ptrY = 0;

    private bool isDone = false;

    private void Start()
    {
        wait = new WaitForSeconds(refreshSpeed);
        StartCoroutine(drawScreen());
    }

    private void fillScreen()
    {
        for (int index = 0; index < 40; index++)
        {
            for (int i = 0; i < 25; i++)
            {
                Screen.instance.screenMemory[index, i] = char.Parse(" ");
            }
        }
    }

    private IEnumerator drawScreen()
    {
        if (!(Screen.instance.screenMemory[ptrX, ptrY] == char.Parse(" ")))
        {
            Screen.instance.display.text = Screen.instance.display.text + Screen.instance.screenMemory[ptrX, ptrY].ToString();
        }
        else
        {
            Screen.instance.display.text = Screen.instance.display.text + " ";
        }


        Debug.Log(Screen.instance.screenMemory[ptrX, ptrY]);
        ptrX++;
        if (ptrX > 39)
        {
            ptrX = 0;
            ptrY++;
            if (ptrY > 24)
            {
                isDone = true;
                Debug.Log(ptrY);
            }
        }

        yield return wait;
        if (isDone == false)
        {
            StartCoroutine(drawScreen());
        }
        else
        {
            Screen.instance.display.text = "";
            isDone = false;
            ptrX = 0;
            ptrY = 0;
            StartCoroutine(drawScreen());
        }
    }
}
