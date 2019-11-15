using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeWatcher : MonoBehaviour
{
    [SerializeField] private NewSpiderBehavior currentEnemy;
    private bool isNotTouchingGround = false;
    private int groundsTouching = 0;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (groundsTouching <= 0)
        {
            currentEnemy.notifyOfEdge(false);
        }
        groundsTouching++;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        groundsTouching--;
        if (groundsTouching <= 0)
        {
            currentEnemy.notifyOfEdge(true);
        }
    }
}
