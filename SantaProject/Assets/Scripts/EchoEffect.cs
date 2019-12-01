using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    public bool shouldEcho = false;

    [SerializeField] private float maxTimeInbetweenSpawns = .25f;
    [SerializeField] private GameObject whatToSpawn;

    private float timeSinceLastSpawn = .25f;

    private void Start()
    {
        timeSinceLastSpawn = .01f;
    }
    // Update is called once per frame
    void Update()
    {
        if (shouldEcho== true)
        {
            if (timeSinceLastSpawn <= 0)
            {
                timeSinceLastSpawn = maxTimeInbetweenSpawns;
                //spawn echo
                Instantiate(whatToSpawn, transform.position, transform.rotation);
            }
            timeSinceLastSpawn -= Time.deltaTime;
        }
    }
}
