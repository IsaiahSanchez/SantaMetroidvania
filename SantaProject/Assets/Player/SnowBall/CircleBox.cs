using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBox : MonoBehaviour
{
    [SerializeField] private Vector2 location;
    [SerializeField] private SnowBall mySnowball;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (location != null)
        {
            mySnowball.snowBallHitBox(location);
            
        }
        else
        {
            mySnowball.snowBallHitBox(Vector2.zero);
        }
    }

}
