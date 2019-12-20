using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField]private Transform respawnLocation;
    [SerializeField] private GameObject checkpointHitParticle;

    private void OnTriggerEnter2D(Collider2D target)
    {
        Instantiate(checkpointHitParticle, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        AudioManager.instance.PlaySound("Checkpoint");
        GameDataManager.instance.saveGame(new Vector2(respawnLocation.position.x, respawnLocation.position.y-.5f));
        GameManager.Instance.mainPlayer.setPlayerHealthMax(0, false);
    }
}
