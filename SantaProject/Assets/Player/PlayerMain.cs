using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] private Transform snowballThrowLocation;
    [SerializeField] private GameObject SnowBallPrefab;
    [SerializeField] private float SnowBallThrowSpeed;
    [SerializeField] private float timeAfterTeleportToEnableThrowing;
    [SerializeField] private Animator anim;

    private bool canThrowSnowball = true;
    private Coroutine mainWaitCoroutine;
    private SnowBall currentSnowball;
    private float health = 100f;
    public bool hasDoubleJumpPower = false;
    public bool hasDashPower = false;
    public bool hasSnowBallPower = false;
    public int numberOfPresentsCollected = 0;

    private PlayerMovement myMovement;

    private void Awake()
    {
       
        myMovement = GetComponent<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.updatePlayerHealthText(health);
        UIManager.Instance.showPowerup("WASD or Arrow Keys to move, K or Space to jump");
        if (hasDoubleJumpPower == true)
        {
            hasDoubleJumpPower = false;
            givePower(0);
        }

        if (hasDashPower == true)
        {
            hasDashPower = false;
            givePower(1);
        }

        if (hasSnowBallPower == true)
        {
            hasSnowBallPower = false;
            givePower(2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canThrowSnowball == true)
        {
            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Return))
            {
                if (hasSnowBallPower == true)
                {
                    throwSnowball();
                }
            }
        }
    }

    public void givePower(int powerIndex)
    {
        string nameAndDescription = "";
        switch (powerIndex)
        {
            case 0:
                if (hasDoubleJumpPower == false)
                {
                    nameAndDescription = "You have gained the Double Jump power, press Jump a second time while in mid air to jump a second time!";
                    hasDoubleJumpPower = true;
                    myMovement.notifyOfDoubleJumpGet();
                    UIManager.Instance.showPowerup(nameAndDescription);
                }
                break;
            case 1:
                if (hasDashPower == false)
                {
                    nameAndDescription = "You have gained the Dash power, press O to dash the direction you are aiming with WASD!";
                    hasDashPower = true;
                    UIManager.Instance.showPowerup(nameAndDescription);
                }
                break;
            case 2:
                if(hasSnowBallPower == false)
                {
                    nameAndDescription = "You have gained the Snow Ball Teleport power, press J to throw a snowball that will teleport you to where it lands!";
                    hasSnowBallPower = true;
                    UIManager.Instance.showPowerup(nameAndDescription);
                }
                break;
        }

    }
    private void throwSnowball()
    {
        canThrowSnowball = false;
        //spawn snowball
        if (currentSnowball == null)
        {
            GameObject temp = Instantiate(SnowBallPrefab, snowballThrowLocation.position,Quaternion.identity,snowballThrowLocation);
            currentSnowball = temp.GetComponent<SnowBall>();
            currentSnowball.gameObject.SetActive(false);
        }
        else
        {
            currentSnowball.transform.position = snowballThrowLocation.position;
            currentSnowball.transform.parent = snowballThrowLocation;
        }
        anim.SetTrigger("ThrowSnowball");

    }

    public void actuallyThrow()
    {
        currentSnowball.gameObject.SetActive(true);
        //move it relative right
        currentSnowball.init(this, snowballThrowLocation.right, SnowBallThrowSpeed);
        //unparent it
        currentSnowball.transform.parent = null;
        //block throwing another
        //listen for it to land to teleport and enable throwing it again.
        if (mainWaitCoroutine != null)
        {
            StopCoroutine(mainWaitCoroutine);
        }
        mainWaitCoroutine = StartCoroutine(snowBallTimeOut());
    }

    public void snowBallHasHit(Vector2 HitLocation)
    {
        myMovement.TeleportToSnowBallHit(HitLocation);
        if (mainWaitCoroutine != null)
        {
            StopCoroutine(mainWaitCoroutine);
        }
        StartCoroutine(waitToEnableThrowing());
    } 

    private IEnumerator waitToEnableThrowing()
    {
        yield return new WaitForSeconds(timeAfterTeleportToEnableThrowing);
        canThrowSnowball = true;
    }

    private IEnumerator snowBallTimeOut()
    {
        yield return new WaitForSeconds(4f);
        currentSnowball.disableSnowball();
        canThrowSnowball = true;
    }

    public void TakeDamage(float amount, int direction)
    {
        health -= amount;
        UIManager.Instance.updatePlayerHealthText(health);
        //knockback  need to create function in playermovement script.
        myMovement.getKnockedBack(direction);
        if (health <= 0)
        {
            //die
            GameManager.Instance.StartGameOver();
        }
    }

}
