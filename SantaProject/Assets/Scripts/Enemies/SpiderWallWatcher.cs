using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWallWatcher : MonoBehaviour
{
    [SerializeField] private NewSpiderBehavior mySpiderEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13 || collision.gameObject.layer == 12)
        {
            mySpiderEnemy.notifyOfWall(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13 || collision.gameObject.layer == 12)
        {
            mySpiderEnemy.notifyOfWall(false);
        }
    }
}
