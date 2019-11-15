using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpiderBehavior : Enemy
{
    private enum spiderState {Chasing, Roaming, Dead}

    [SerializeField] private float amountToWaitBeforeRoamChecking = 2f;
    [SerializeField] private float MovementSpeed = 10f;
    [SerializeField] private GameObject damageBoxRef, playerDetectorRef, WeakPointRef;
    [SerializeField] private Sprite defaultSprite, arachnaephobiaSprite;
    [SerializeField] private SpriteRenderer mySprite;

    private EnemyFacingHandler myFacing;
    private Animator myAnimator;
    private spiderState currentState = spiderState.Roaming;
    private int CurrentDirection = 1;
    private bool foundEdge = false, foundWall = false, isImpeded = false, hasChanged = false;
    private float MaxMovementSpeed;
    private bool isArachnaephobiaMode = false;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        myFacing = GetComponent<EnemyFacingHandler>();
        MaxMovementSpeed = MovementSpeed;

    }

    private void OnEnable()
    {
        
    }

    private void Start()
    {

        myFacing.directionHasChanged(CurrentDirection);
    }
    // Update is called once per frame
    void Update()
    {
        handleFacingDirection();
        changeSprite();
        switch (currentState)
        {
            case spiderState.Roaming:
                if (isImpeded == true)
                {
                    TurnAround();
                }
                myBody.velocity = new Vector2(CurrentDirection * MovementSpeed, myBody.velocity.y);
                break;

            case spiderState.Chasing:
                
                if (GameManager.Instance.mainPlayer.transform.position.x < transform.position.x)
                { 
                    CurrentDirection = -1;
                    myFacing.directionHasChanged(CurrentDirection);
                }
                else
                {
                    CurrentDirection = 1;
                    myFacing.directionHasChanged(CurrentDirection);
                }
                myBody.velocity = new Vector2(CurrentDirection * MovementSpeed, myBody.velocity.y);

                break;

            case spiderState.Dead:
                myBody.velocity = Vector2.zero;
                break;
        } 
    }

    private void changeSprite()
    {
        if (GameManager.Instance.arachnaephobiaModeEnabled == true)
        {
            if (isArachnaephobiaMode != GameManager.Instance.arachnaephobiaModeEnabled)
            {
                mySprite.sprite = arachnaephobiaSprite;
                isArachnaephobiaMode = GameManager.Instance.arachnaephobiaModeEnabled;
            }
        }
        else
        {
            if (isArachnaephobiaMode != GameManager.Instance.arachnaephobiaModeEnabled)
            {
                mySprite.sprite = arachnaephobiaSprite;
                isArachnaephobiaMode = GameManager.Instance.arachnaephobiaModeEnabled;
            }
        }
    }

    private void handleFacingDirection()
    {

        

        if (foundEdge == true || foundWall == true)
        {
            isImpeded = true;
            if (currentState == spiderState.Chasing)
            {
                MovementSpeed = 0;
            }
        }
        else
        {
            isImpeded = false;
            if (currentState == spiderState.Chasing)
            {
                MovementSpeed = MaxMovementSpeed;
            }
        }

    }

    public void TurnAround()
    {
        CurrentDirection = CurrentDirection * -1;
        myFacing.directionHasChanged(CurrentDirection);
    }

    public void notifyOfEdge(bool hasFoundEdge)
    {
        foundEdge = hasFoundEdge;

    }

    public void notifyOfWall(bool hasFoundWall)
    {
        foundWall = hasFoundWall;

    }

    private IEnumerator roamingDirectionChanger()
    {
        if (currentState == spiderState.Roaming)
        {
            if (!isImpeded)
            {
                float temp = Random.Range(-1, 1);
                if (temp < 0)
                {
                    temp = -1f;
                }
                else
                {
                    temp = 1f;
                }

                if (CurrentDirection != temp)
                {
                    TurnAround();
                }
            }
        }
        yield return new WaitForSeconds(amountToWaitBeforeRoamChecking);
        StartCoroutine(roamingDirectionChanger());
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
        currentState = spiderState.Dead;
        //change physical hit box to not interact with player
        gameObject.layer = 16;
        //disable damaging box
        damageBoxRef.SetActive(false);
        //disable playerdetector
        playerDetectorRef.SetActive(false);
        //disable weakPoint
        WeakPointRef.SetActive(false);

        //play animation of dying
        myAnimator.SetTrigger("TriggerDeath");
    }

    public override void playerSeen()
    {
        currentState = spiderState.Chasing;
    }

    public override void playerLost()
    {
        currentState = spiderState.Roaming;
        MovementSpeed = MaxMovementSpeed;
    }
}
