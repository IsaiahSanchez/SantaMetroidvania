using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemy : Enemy
{
    private enum SpiderState {roaming, chasing, dead}

    [SerializeField] private float amountToWaitBeforeRoamChecking = 2f;
    [SerializeField] private float normalMovementSpeed = 10f;
    [SerializeField] private GameObject damageBoxRef, playerDetectorRef, WeakPointRef;

    private float movementSpeed = 0;
    private SpiderState currentState = SpiderState.roaming;
    private Vector2 PlayerLocation;
    private bool stateHasChanged = true;
    private Coroutine currentMovementRoutine;
    public int directionMoving = 1;
    private bool hasReachedEdge = false;
    private EnemyFacingHandler facingHandler;
    private GameObject currentPlayerChasing;

    private void Start()
    {
        facingHandler = GetComponent<EnemyFacingHandler>();
        movementSpeed = normalMovementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (stateHasChanged == true)
        {
            switch (currentState)
            {
                case SpiderState.roaming:
                    movementSpeed = normalMovementSpeed;
                    if (hasReachedEdge == true)
                    {
                        StopCoroutine(currentMovementRoutine);
                        hasReachedEdge = false;
                        currentMovementRoutine = StartCoroutine(roam(directionMoving * -1));
                    }
                    else
                    {
                        currentMovementRoutine = StartCoroutine(roam(0));
                    }
                    break;
                case SpiderState.chasing:
                    if (hasReachedEdge == true)
                    {
                        movementSpeed = 0f;
                    }
                    else
                    {
                        movementSpeed = normalMovementSpeed;
                    }
                        currentMovementRoutine = StartCoroutine(chasePlayer());
                    
                    break;
                case SpiderState.dead:
                    myBody.velocity = Vector2.zero;
                    stateHasChanged = false;
                    break;
            }
            stateHasChanged = false;
        }
    }

    private IEnumerator chasePlayer()
    {
        int directionToMove = 1;
        if (currentPlayerChasing.transform.position.x < transform.position.x)
        {
            directionToMove = -1;
        }

        myBody.velocity = new Vector2(directionToMove * movementSpeed, 0);
        directionMoving = directionToMove;
        facingHandler.directionHasChanged(directionMoving);
        yield return new WaitForSeconds(.5f);

        currentMovementRoutine = StartCoroutine(chasePlayer());
    }

    private IEnumerator roam(int directionOverride)
    {
        int directionToMove = 0;
        //pick a random direction
        if (directionOverride == 0)
        {
            while (directionToMove == 0)
            {
                directionToMove = Random.Range(-1, 2);
            }
        }
        else
        {
            directionToMove = directionOverride;
        }

        //move
        myBody.velocity = new Vector2(directionToMove * movementSpeed, 0);
        directionMoving = directionToMove;
        facingHandler.directionHasChanged(directionMoving);

        yield return new WaitForSeconds(amountToWaitBeforeRoamChecking); //wait
        stateHasChanged = true; //make update loop try and roam again. also will happen if gets interrupted by the floor watcher
    }

    public void notifyGroundChange(bool isNotTouchingGround)
    {
        if (currentMovementRoutine != null)
        {
            StopCoroutine(currentMovementRoutine);
        }
        hasReachedEdge = isNotTouchingGround;
        stateHasChanged = true;
    }

    public override bool playerSeen(GameObject playerRef)
    {
        currentPlayerChasing = playerRef;
        currentState = SpiderState.chasing;

        if (currentMovementRoutine != null)
        {
            StopCoroutine(currentMovementRoutine);
        }

        stateHasChanged = true;
        return true;
    }

    public override bool playerLost()
    {
        if (currentMovementRoutine != null)
        {
            StopCoroutine(currentMovementRoutine);
        }

        currentState = SpiderState.roaming;
        stateHasChanged = true;
        return true;
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
        currentState = SpiderState.dead;
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
    }

}
