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

    private Rigidbody2D myBody;
    private PlayerMain myPlayer;

    private bool canJump = true;
    private int numberOfJumps = 0;
    private bool canDash = true;
    private bool isJumping = false;
    private bool isDashing = false;
    private bool isBeingKnocked = false;
    private Coroutine currentDashTimer;
    private MovementState currentMovementState = MovementState.idle;

    private Coroutine currentWaitingForFloor;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myPlayer = GetComponent<PlayerMain>();
        numberOfJumps = MaxNumberOfJumps;
    }

    private void Update()
    {
        if (isDashing == false && isBeingKnocked == false)
        {
            handleJumpInput();
            handleDashInput();
        }
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
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0)
            {
                playerAnimator.SetBool("IsRunning", true);
                myBody.velocity = new Vector2((Input.GetAxisRaw("Horizontal") * WalkSpeed * 100f) * Time.deltaTime, myBody.velocity.y);
            }
            else
            {
                playerAnimator.SetBool("IsRunning", false);
                myBody.velocity = new Vector2(0, myBody.velocity.y);
            }
        }
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

    private void handleJumpInput()
    {
       
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.K))
        {
            jump();
        }


        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.K))
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

    private void handleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.LeftShift))
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
        playerAnimator.SetBool("IsJumping", true);
        myDashEcho.shouldEcho = true;
        canDash = false;
        myBody.gravityScale = 0f;
        isDashing = true;
        if (currentDashTimer != null)
        {
            StopCoroutine(currentDashTimer);
        }
        currentDashTimer = StartCoroutine(DashCount());
        myBody.velocity = new Vector2(0, 0);
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
        {
            Vector2 temp = (new Vector2(Input.GetAxisRaw("Horizontal") * dashSpeed, Input.GetAxisRaw("Vertical") * dashSpeed));
            myBody.velocity = (temp.normalized*dashSpeed);
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
        myBody.velocity = Vector2.zero;
        transform.position = SnowBallHitLocation + new Vector2(0,.75f);
        playerAnimator.SetBool("IsJumping", false);
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
