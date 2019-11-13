using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWallWatcher : MonoBehaviour
{
    [SerializeField] private SpiderEnemy mySpiderEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13 || collision.gameObject.layer == 12)
        {
            mySpiderEnemy.notifyGroundChange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13 || collision.gameObject.layer == 12)
        {
            mySpiderEnemy.notifyGroundChange(false);
        }
    }
}
