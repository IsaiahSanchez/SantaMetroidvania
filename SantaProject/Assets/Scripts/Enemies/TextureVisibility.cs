using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureVisibility : MonoBehaviour
{
    [SerializeField] private GameObject sprite;

    private void Awake()
    {
        sprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        sprite.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        sprite.SetActive(false);
    }
}
