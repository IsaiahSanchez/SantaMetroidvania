using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform openPosition, closedPosition;
    [SerializeField] private bool isOpening;
    [SerializeField] private float speed = 10f;

    protected bool isAtIntendedPosition = true;
    private Vector2 currentAimPosition = Vector2.zero;
    private Rigidbody2D myBody;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (isAtIntendedPosition == false)
        {
            Vector2 temp = new Vector2(0, currentAimPosition.y - transform.position.y);
            myBody.velocity = temp.normalized*speed;

            if (transform.position.y > currentAimPosition.y - .05f && transform.position.y < currentAimPosition.y + .05f)
            {
                transform.position = currentAimPosition;
                myBody.velocity = Vector2.zero;
                isAtIntendedPosition = true;
            }
        }
    }

    public void openDoor()
    {
        if (currentAimPosition != new Vector2(openPosition.position.x, openPosition.position.y))
        {
            currentAimPosition = openPosition.position;
            if (transform.position.y > currentAimPosition.y - .05f && transform.position.y <= currentAimPosition.y + .05f)
            {
                AudioManager.instance.PlaySound("Door");
            }
            isAtIntendedPosition = false;
        }
    }

    public void closeDoor()
    {
        if (currentAimPosition != new Vector2(closedPosition.position.x, closedPosition.position.y))
        {
            currentAimPosition = closedPosition.position;
            if (!(transform.position.y > currentAimPosition.y - .05f && transform.position.y <= currentAimPosition.y + .05f))
            {
                AudioManager.instance.PlaySound("Door");
            }
            isAtIntendedPosition = false;
        }
    }
}
