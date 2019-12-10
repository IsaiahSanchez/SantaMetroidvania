using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum MovementState { idle, walking, jumping, dashing, knockedBack }

    [SerializeField] private float WalkSpeed = 5f;
    [SerializeField]private float jumpSpeed = 1f;
    [SerializeField] private float jumpUpGravity = .25f;
    [SerializeField] private float fallDownGravity = 2.5f;
    [SerializeField] private float jumpTime = .5f;
    [SerializeField] private float dashTime = 1f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashCooldownAmount = 5f;
    [SerializeField] private int MaxNumberOfJumps = 1;
    [SerializeField] private float knockBackAmount;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private GameObject myHitBox;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private FeetTrigger myFeet;
    [SerializeField] private EchoEffect myDashEcho;
    [SerializeField] private GameObject teleportParticle;

    private Rigidbody2D myBody;
    private PlayerMain myPlayer;

    private bool canJump = true;
    private bool canMove = true;
    private int numberOfJumps = 0;
    private bool canDash = true;
    private bool isJumping = false;
    private bool isDashing = false;
    private bool isBeingKnocked = false;
    private Vector2 inputDirection = Vector2.zero;
    private Coroutine currentDashTimer;
    private Coroutine currentTeleportWait;
    private MovementState currentMovementState = MovementState.idle;

    private Coroutine currentWaitingForFloor;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myPlayer = GetComponent<PlayerMain>();
        numberOfJumps = MaxNumberOfJumps;
    }

    private void FixedUpdate()
    {
        if (isBeingKnocked == false)
        {
            handleXMovement();
        }
    }

    private void handleXMovement()
    {
       
            if (isDashing == false)
            {

                if (inputDirection.x != 0)
                {
                if (canMove == true)
                {
                    playerAnimator.SetBool("IsRunning", true);
                    float MovementDirection = 0;
                    if (inputDirection.x > 0)
                    {
                        MovementDirection = 1;
                    }
                    else
                    {
                        MovementDirection = -1;
                    }

                    if (Mathf.Abs(inputDirection.x) > .25)
                    {
                        MovementDirection = inputDirection.x;
                    }

                    myBody.velocity = new Vector2((MovementDirection * WalkSpeed * 100f) * Time.deltaTime, myBody.velocity.y);
                }
                }
                else
                {
                    playerAnimator.SetBool("IsRunning", false);
                    myBody.velocity = new Vector2(0, myBody.velocity.y);
                }
            }
    }

    public void setMovmentVector(Vector2 direction)
    {
        inputDirection = direction;
    }

    public void getKnockedBack(int direction)
    {
        isBeingKnocked = true;
        //start coroutine
        StartCoroutine(lossInputTimer());
        //add force
        myBody.velocity = Vector2.zero;
        myBody.AddForce(new Vector2(1 * direction,.75f).normalized * knockBackAmount);
        //disable hit box temporarily
        myPlayer.canTakeDamage = false;
    }
    private IEnumerator lossInputTimer()
    {
        yield return new WaitForSeconds(knockBackDuration);
        //regain control and gain hitbox again
        isBeingKnocked = false;
        myPlayer.canTakeDamage = true;
    }

    public void handleStartJump()
    {
        if (isDashing == false && isBeingKnocked == false)
        {
            jump();
        }
    }
    public void handleStopJump()
    {
        if (isDashing == false && isBeingKnocked == false)
        {
            stopJumping();
        }
    }
    public void hasLanded()
    {
        AudioManager.instance.PlaySound("Land");
        resetPowers();
        if (currentWaitingForFloor != null)
        {
            StopCoroutine(currentWaitingForFloor);
        }

        playerAnimator.SetBool("IsJumping", false);

    }

    private void jump()
    {
        if (canJump == true)
        {
            AudioManager.instance.PlaySound("Jump");
            playerAnimator.SetBool("IsJumping", false);
            myBody.velocity = new Vector2(myBody.velocity.x, 0);
            myBody.AddForce(new Vector2(0, jumpSpeed * 4f));
            myBody.gravityScale = jumpUpGravity;
            numberOfJumps--;
            StartCoroutine(countTilDoneJumping());
            myPlayer.spawnLandingDust();

            if (numberOfJumps <= 0)
            {
                canJump = false;
            }
        }
    }
    private IEnumerator countTilDoneJumping()
    {
        yield return new WaitForSeconds(jumpTime * .1f);
        playerAnimator.SetBool("IsJumping", true);
        yield return new WaitForSeconds(jumpTime * .9f);
        if (isDashing != true)
        {
            stopJumping();
        }
    }
    private void stopJumping()
    {
        //myBody.AddForce(new Vector2(0, -(jumpSpeed*10)/4));
        myBody.gravityScale = fallDownGravity;
    }

    public void fallOffLedge()
    {
        playerAnimator.SetBool("IsJumping", true);
        //currentWaitingForFloor = StartCoroutine(waitForFloor());
    }
    private IEnumerator waitForFloor()
    {
        yield return new WaitForSeconds(.05f);
        playerAnimator.SetBool("IsJumping", true);
    }

    public void resetPowers()
    {
        canJump = true;
        numberOfJumps = MaxNumberOfJumps;
        canDash = true;
        myPlayer.spawnLandingDust();
    }
    

    public void notifyOfDoubleJumpGet()
    {
        MaxNumberOfJumps = 2;
        numberOfJumps = MaxNumberOfJumps;
    }

    public void handleDashInput()
    {
        if (isDashing == false && isBeingKnocked == false)
        {
            if (myPlayer.hasDashPower == true)
            {
                if (canDash == true)
                {
                    Dash();
                }
            }
        }
    }

    private void Dash()
    {
        AudioManager.instance.PlaySound("Dash");
        playerAnimator.ResetTrigger("Dash");
        playerAnimator.SetTrigger("Dash");
        myDashEcho.shouldEcho = true;
        myDashEcho.GetComponent<ParticleSystem>().Play();
        canDash = false;
        myBody.gravityScale = 0f;
        isDashing = true;
        if (currentDashTimer != null)
        {
            StopCoroutine(currentDashTimer);
        }
        currentDashTimer = StartCoroutine(DashCount());
        myBody.velocity = new Vector2(0, 0);
        if (Mathf.Abs(inputDirection.x) > 0 || Mathf.Abs(inputDirection.y) > 0)
        {
            Vector2 directionToDash = (new Vector2(inputDirection.x, inputDirection.y));
            Debug.Log(inputDirection);
            myBody.velocity = (directionToDash.normalized*dashSpeed);
        }
        else
        {
            myBody.velocity = new Vector2(1 * dashSpeed, 0);
        }
    }
    private IEnumerator DashCount()
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        myBody.velocity = new Vector2(myBody.velocity.x, myBody.velocity.y * .25f);
        stopJumping();
        if (myFeet.isTouchingGround)
        {
            hasLanded();
        }
        myDashEcho.shouldEcho = false;
    }

    public void TeleportToSnowBallHit(Vector2 SnowBallHitLocation)
    {
        if (currentTeleportWait != null)
        {
            StopCoroutine(currentTeleportWait);
        }
        currentTeleportWait = StartCoroutine(waitAfterTeleport(.1f));
        myBody.velocity = Vector2.zero;
        Instantiate(teleportParticle, new Vector2(transform.position.x, transform.position.y - .5f), Quaternion.identity);
        transform.position = SnowBallHitLocation + new Vector2(0,.75f);
        Instantiate(teleportParticle, new Vector2(transform.position.x,transform.position.y-.5f), Quaternion.identity);
        playerAnimator.SetBool("IsJumping", false);
    }

    private IEnumerator waitAfterTeleport(float waitTime)
    {
        canMove = false;
        yield return new WaitForSeconds(waitTime);
        canMove = true;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.relativeVelocity.magnitude > 1f)
    //    {
    //        if (isDashing == true)
    //        {
    //            StopCoroutine(currentDashTimer);
    //            isDashing = false;
    //            myBody.velocity = new Vector2(myBody.velocity.x, myBody.velocity.y * .05f);
    //            stopJumping();
    //            if (myFeet.isTouchingGround)
    //            {
    //                hasLanded();
    //            }
    //        }
    //    }
    //}
}
