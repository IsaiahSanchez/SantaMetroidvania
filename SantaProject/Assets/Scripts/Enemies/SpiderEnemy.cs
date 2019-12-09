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
    private bool ShouldTurn = false;
    private EnemyFacingHandler facingHandler;
    private GameObject currentPlayerChasing;

    private void Start()
    {
        facingHandler = GetComponent<EnemyFacingHandler>();
        movementSpeed = normalMovementSpeed;
    }


    private void OnEnable()
    {
        if (currentState == SpiderState.dead)
        {
            die();
        }
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
                    if (ShouldTurn == true)
                    {
                        StopCoroutine(currentMovementRoutine);
                        ShouldTurn = false;
                        currentMovementRoutine = StartCoroutine(roam(directionMoving * -1));
                    }
                    else
                    {
                        currentMovementRoutine = StartCoroutine(roam(0));
                    }
                    break;
                case SpiderState.chasing:
                    
                    if (ShouldTurn == true)
                    {
                        stateHasChanged = true;
                    }
                    else
                    {
                        currentMovementRoutine = StartCoroutine(chasePlayer());
                    }
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

    public void notifyGroundChange(bool shouldFlip)
    {
        if (shouldFlip == true)
        {
            ShouldTurn = shouldFlip;
            stateHasChanged = true;
        }
    }

    public override void playerSeen()
    {
        //currentPlayerChasing = playerRef;
        currentState = SpiderState.chasing;

        if (currentMovementRoutine != null)
        {
            StopCoroutine(currentMovementRoutine);
        }

        stateHasChanged = true;
    }

    public override void playerLost()
    {
        if (currentMovementRoutine != null)
        {
            StopCoroutine(currentMovementRoutine);
        }

        currentState = SpiderState.roaming;
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
        if (currentMovementRoutine != null)
        {
            StopCoroutine(currentMovementRoutine);
        }
        stateHasChanged = false;
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
        StartCoroutine(freezeAfterDeath());
    }

    private IEnumerator freezeAfterDeath()
    {
        yield return new WaitForSeconds(.5f);
        myBody.velocity = Vector2.zero;
        myBody.gravityScale = 0f;
    }

}
