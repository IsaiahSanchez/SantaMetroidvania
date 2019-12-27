using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureVisibility : MonoBehaviour
{
    [SerializeField] private GameObject sprite;
    [SerializeField] private Animator anim;
    [SerializeField] private NewSpiderBehavior spider;

    private void Awake()
    {
        sprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        sprite.SetActive(true);
        if (spider.currentState != NewSpiderBehavior.spiderState.Dead)
        {
            anim.ResetTrigger("Walk");
            anim.SetTrigger("Walk");
        }
        else
        {
                anim.ResetTrigger("Dead");
                anim.SetTrigger("Dead");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        sprite.SetActive(false);
    }
}
