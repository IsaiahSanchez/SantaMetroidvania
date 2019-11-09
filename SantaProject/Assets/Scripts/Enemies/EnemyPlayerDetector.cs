using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerDetector : MonoBehaviour
{
    [SerializeField] private Enemy myEnemy;

    private Coroutine currentPlayerSeenCo;

    private void OnTriggerEnter2D(Collider2D target)
    {
        currentPlayerSeenCo = StartCoroutine(alertEnemyOfPlayerSeen(target.gameObject));
    }

    private void OnTriggerExit2D(Collider2D target)
    {
        if (gameObject.activeSelf == true)
        {
            currentPlayerSeenCo = StartCoroutine(alertEnemyOfPlayerLost());
        }
    }

    private IEnumerator alertEnemyOfPlayerSeen(GameObject playerRef)
    {
        yield return new WaitForEndOfFrame();
        bool temp = myEnemy.playerSeen(playerRef);
        if (temp == false)
        {
            StartCoroutine(alertEnemyOfPlayerSeen(playerRef));
        }
    }

    private IEnumerator alertEnemyOfPlayerLost()
    {
        StopCoroutine(currentPlayerSeenCo);
        yield return new WaitForEndOfFrame();
        bool temp = myEnemy.playerLost();
        if (temp == false)
        {
            StartCoroutine(alertEnemyOfPlayerLost());
        }
    }
}
