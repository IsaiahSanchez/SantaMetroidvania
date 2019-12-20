using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] GameObject whatEnables;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        whatEnables.SetActive(true);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        whatEnables.SetActive(false);
        
    }
}
