﻿using System.Collections;
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
        myEnemy.playerSeen();

    }

    private IEnumerator alertEnemyOfPlayerLost()
    {
        if (currentPlayerSeenCo != null)
        {
            StopCoroutine(currentPlayerSeenCo);
        }
        yield return new WaitForEndOfFrame();
        myEnemy.playerLost();

    }
}
