using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemy : Enemy
{
    private enum BatStates {sleeping, chasing, returning}

    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float timeChasing = 1f;
    [SerializeField] float timeWaiting = .75f;
    [SerializeField] float bufferCircle = .25f;
    [SerializeField] GameObject damageBoxRef, WeakPointRef, playerDetectorRef;


    private BatStates currentBatState = BatStates.sleeping;
    private bool stateHasChanged = false;
    private Vector2 startingLocation = Vector2.zero;
    private GameObject playerReference;
    private Coroutine currentMovementRoutine;


    private void Start()
    {

    }
    private void OnEnable()
    {
        startingLocation = transform.position;
    }

    private void Update()
    {
        if (playerReference == null)
        {
            playerReference = GameManager.Instance.mainPlayer.gameObject;
        }
        if (stateHasChanged == true)
        {
            switch (currentBatState)
            {
                case BatStates.sleeping:
                    myBody.velocity = Vector2.zero;
                    stateHasChanged = false;
                    return;
                case BatStates.chasing:
                    stateHasChanged = false;
                    currentMovementRoutine = StartCoroutine(chasePlayer());
                    return;
                case BatStates.returning:
                    stateHasChanged = false;
                    currentMovementRoutine = StartCoroutine(moveEnemyHome());
                    return;
            }
        }
    }

    private IEnumerator moveEnemyHome()
    {
        myBody.velocity = (startingLocation - new Vector2(transform.position.x, transform.position.y)).normalized * movementSpeed;
        yield return new WaitForSeconds(.05f);

        if (startingLocation.x < transform.position.x + bufferCircle && startingLocation.x > transform.position.x - bufferCircle &&
            startingLocation.y < transform.position.y + bufferCircle && startingLocation.y > transform.position.y - bufferCircle)
        {
            
            currentBatState = BatStates.sleeping;
            stateHasChanged = true;
        }
        else
        {
            currentMovementRoutine = StartCoroutine(moveEnemyHome());
        }
    }

    private IEnumerator chasePlayer()
    {
        myBody.velocity = (new Vector2(playerReference.transform.position.x, playerReference.transform.position.y) - new Vector2(transform.position.x,transform.position.y)).normalized * movementSpeed;
        yield return new WaitForSeconds(timeChasing);
        currentMovementRoutine = StartCoroutine(waitWhileChasing());
    }

    private IEnumerator waitWhileChasing()
    {
        myBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(timeWaiting);
        currentMovementRoutine = StartCoroutine(chasePlayer());
    }

    public override void playerSeen()
    {
        if (currentMovementRoutine != null)
        {
            StopCoroutine(currentMovementRoutine);
        }
        currentBatState = BatStates.chasing;
        stateHasChanged = true;
    }

    public override void playerLost()
    {
        if (currentMovementRoutine != null)
        {
            StopCoroutine(currentMovementRoutine);
        }
        currentBatState = BatStates.returning;
        stateHasChanged = true;
    }

    public override void damageEnemy()
    {
        hitPoints--;
        if (hitPoints <= 0)
        {
            die();
        }
    }

    protected override void die()
    {
        //stop enemy
        StopCoroutine(currentMovementRoutine);
        currentBatState = BatStates.sleeping;
        stateHasChanged = true;
        myBody.gravityScale = 2.5f;
        //change physical hit box to not interact with player
        gameObject.layer = 16;
        //disable damaging box
        damageBoxRef.SetActive(false);
        //disable playerdetector
        playerDetectorRef.SetActive(false);
        //disable weakPoint
        WeakPointRef.SetActive(false);
        base.die();
    }
}
