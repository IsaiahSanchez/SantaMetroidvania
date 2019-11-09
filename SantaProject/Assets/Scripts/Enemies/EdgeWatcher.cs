using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeWatcher : MonoBehaviour
{
    [SerializeField] private SpiderEnemy currentEnemy;
    private bool isNotTouchingGround = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isNotTouchingGround == true)
        {
            isNotTouchingGround = false;
            currentEnemy.notifyGroundChange(isNotTouchingGround);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isNotTouchingGround == false)
        {
            isNotTouchingGround = true;
            currentEnemy.notifyGroundChange(isNotTouchingGround);
        }
    }
}
