using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetTrigger : MonoBehaviour
{
    [SerializeField] private PlayerMovement myMovement;

    public bool isTouchingGround { get; private set;}
    private int numberOfFloorsTouching = 0;

    private void Update()
    {
        if (numberOfFloorsTouching > 0)
        {
            isTouchingGround = true;
        }
        else
        {
            isTouchingGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            StartCoroutine(waitToTellTouchingGround());
        }
        else if (collision.gameObject.layer == 10)
        {
            Rigidbody2D collisionBody = myMovement.GetComponent<Rigidbody2D>();
            myMovement.resetPowers();
            collisionBody.velocity = new Vector2(collisionBody.velocity.x, collisionBody.velocity.y * .05f);
            collisionBody.AddForce(new Vector2(0, 450f));
        }

    }

    private IEnumerator waitToTellTouchingGround()
    {
        yield return new WaitForEndOfFrame();
        numberOfFloorsTouching++;
        if (numberOfFloorsTouching == 1)
        {
            myMovement.hasLanded();
        }
        Debug.Log(isTouchingGround);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            numberOfFloorsTouching--;
            if (numberOfFloorsTouching <= 0)
            {
                myMovement.fallOffLedge();
            }
            Debug.Log(isTouchingGround);
        }
    }
}
