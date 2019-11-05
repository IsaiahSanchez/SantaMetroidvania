using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum MovementState { idle, walking, jumping, dashing }

    [SerializeField] private float WalkSpeed = 5f;
    [SerializeField]private float jumpSpeed = 1f;
    [SerializeField] private float jumpTime = .5f;
    [SerializeField] private float dashTime = 1f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private int MaxNumberOfJumps = 1;

    private Rigidbody2D myBody;
    private bool canJump = true;
    private bool isJumping = false;
    private int numberOfJumps = 0;
    private bool canDash = true;
    private bool isDashing = false;
    private Coroutine currentDashTimer;
    private MovementState currentMovementState = MovementState.idle;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        numberOfJumps = MaxNumberOfJumps;
    }

    private void Update()
    {
        if (isDashing == false)
        {
            handleJumpInput();
            handleDashInput();
        }
    }

    private void FixedUpdate()
    {
        handleXMovement();
    }

    private void handleXMovement()
    {
        if (isDashing == false)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0)
            {
                myBody.velocity = new Vector2((Input.GetAxisRaw("Horizontal") * WalkSpeed * 100f) * Time.deltaTime, myBody.velocity.y);
            }
            else
            {
                myBody.velocity = new Vector2(0, myBody.velocity.y);
            }
        }
    }



    private void jump()
    {
        if (canJump == true)
        {
            myBody.velocity = new Vector2(myBody.velocity.x, 0);
            myBody.AddForce(new Vector2(0, jumpSpeed * 4f));
            myBody.gravityScale = 0f;
            numberOfJumps--;
            StartCoroutine(countTilDoneJumping());

            if (numberOfJumps <= 0)
            {
                canJump = false;
            }
        }
    }
    private IEnumerator countTilDoneJumping()
    {
        yield return new WaitForSeconds(jumpTime);
        if (isDashing != true)
        {
            stopJumping();
        }
    }
    private void stopJumping()
    {
        //myBody.AddForce(new Vector2(0, -(jumpSpeed*10)/4));
        myBody.gravityScale = 2.5f;
    }

    private void handleJumpInput()
    {
       
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.K))
        {
            jump();
        }


        if (Input.GetKeyUp(KeyCode.Space))
        {
            stopJumping();
        }
    }
    public void hasLanded()
    {
        canJump = true;
        numberOfJumps = MaxNumberOfJumps;
        canDash = true;
    }



    private void handleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Dash();
        }
    }
    private void Dash()
    {
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
        myBody.velocity = new Vector2(myBody.velocity.x, myBody.velocity.y * .05f);
        stopJumping();
    }


    public void TeleportToSnowBallHit(Vector2 SnowBallHitLocation)
    {
        myBody.velocity = Vector2.zero;
        transform.position = SnowBallHitLocation + new Vector2(0,.5f);
    }
}
