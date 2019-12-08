using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform openPosition, closedPosition;
    [SerializeField] private bool isOpening;
    [SerializeField] private float speed = 10f;

    private bool isAtIntendedPosition = true;
    private float timeToOpen = 1f;
    private Vector2 currentAimPosition = Vector2.zero;
    private Rigidbody2D myBody;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    private void Update()
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
            isAtIntendedPosition = false;
        }
    }

    public void closeDoor()
    {
        if (currentAimPosition != new Vector2(closedPosition.position.x, closedPosition.position.y))
        {
            currentAimPosition = closedPosition.position;
            isAtIntendedPosition = false;
        }
    }
}
