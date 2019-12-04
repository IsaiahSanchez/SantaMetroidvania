using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField]private Transform respawnLocation;

    private void OnTriggerEnter2D(Collider2D target)
    {
        GameDataManager.instance.saveGame(new Vector2(respawnLocation.position.x, respawnLocation.position.y));
        GameManager.Instance.mainPlayer.setPlayerHealthMax(0, false);
    }
}
