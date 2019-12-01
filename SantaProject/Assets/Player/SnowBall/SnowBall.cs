using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    private PlayerMain playerRef;
    private Rigidbody2D myBody;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    public void init(PlayerMain playerReference, Vector2 direction, float speed)
    {
        playerRef = playerReference;
        BeginMoving(direction, speed);
    }

    private void BeginMoving(Vector2 dir, float speed)
    {
        myBody.velocity = dir * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerRef.snowBallHasHit(transform.position);
        disableSnowball();
    }

    public void disableSnowball()
    {
        transform.parent = playerRef.transform;
        transform.position = new Vector2(0,-1f);
        gameObject.SetActive(false);
    }
}
