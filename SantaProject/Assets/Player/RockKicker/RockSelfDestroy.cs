using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSelfDestroy : MonoBehaviour
{

    private void Awake()
    {
        StartCoroutine(timeTillDestroy());
    }

    private IEnumerator timeTillDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

}
