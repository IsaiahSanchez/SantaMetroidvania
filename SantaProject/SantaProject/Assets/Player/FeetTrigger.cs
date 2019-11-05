using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetTrigger : MonoBehaviour
{
    [SerializeField] private PlayerMovement myMovement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            myMovement.hasLanded();
        }
        else if (collision.gameObject.layer == 10)
        {
            Rigidbody2D collisionBody = myMovement.GetComponent<Rigidbody2D>();
            collisionBody.velocity = new Vector2(collisionBody.velocity.x, collisionBody.velocity.y * .05f);
            collisionBody.AddForce(new Vector2(0, 450f));
        }

    }
}
