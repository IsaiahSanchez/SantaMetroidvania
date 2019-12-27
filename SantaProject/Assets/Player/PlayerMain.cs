using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] private Transform snowballThrowLocation;
    [SerializeField] private GameObject SnowBallPrefab;
    [SerializeField] private RockKicker myKicker;
    [SerializeField] private ParticleSystem myLandingDust;
    [SerializeField] private float SnowBallThrowSpeed;
    [SerializeField] private float timeAfterTeleportToEnableThrowing;
    [SerializeField] private Animator anim;
    public bool hasStartedThrowingSnowball = false;

    [SerializeField] private List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    private bool wantsToThrowSnowball = false;
    private bool canThrowSnowball = true;
    private Coroutine mainWaitCoroutine;
    private SnowBall currentSnowball;
    private float health = 100f;

    public float maxPlayerHealth = 40f;
    public bool hasDoubleJumpPower = false;
    public bool hasDashPower = false;
    public bool hasSnowBallPower = false;
    public bool isDead = false;
    public int numberOfPresentsCollected = 0;
    public bool canTakeDamage = true;

    private PlayerMovement myMovement;

    private void Awake()
    {
        health = maxPlayerHealth;
        myMovement = GetComponent<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.updatePlayerHealthText(health, maxPlayerHealth);

        if (hasDoubleJumpPower == true)
        {
            hasDoubleJumpPower = false;
            givePower(0, false);
        }

        if (hasDashPower == true)
        {
            hasDashPower = false;
            givePower(1, false);
        }

        if (hasSnowBallPower == true)
        {
            hasSnowBallPower = false;
            givePower(2, false);
        }
    }

    private void Update()
    {
        if (myMovement.isDashing == false)
        {
            if (wantsToThrowSnowball == true)
            {
                tryThrowSnowball();
            }
        }
    }

    public void setPlayerHealthMax(float maxHealth, bool shouldUpdateMaxHealth)
    {
        if (shouldUpdateMaxHealth == true)
        {
            maxPlayerHealth = maxHealth;
        }

        health = maxPlayerHealth;
        UIManager.Instance.updatePlayerHealthText(health, maxPlayerHealth);
    }

    public void givePower(int powerIndex, bool shouldShowText)
    {
        string nameAndDescription = "";
        switch (powerIndex)
        {
            case 0:
                if (hasDoubleJumpPower == false)
                {
                    nameAndDescription = "Press Jump a second time while in mid air to jump a second time!";
                    hasDoubleJumpPower = true;
                    UIManager.Instance.hasDoubleJump();
                    myMovement.notifyOfDoubleJumpGet();
                }
                break;
            case 1:
                if (hasDashPower == false)
                {
                    nameAndDescription = "Press O or B on controller to dash the direction you are moving";
                    UIManager.Instance.hasDash();
                    hasDashPower = true;
                }
                break;
            case 2:
                if(hasSnowBallPower == false)
                {
                    nameAndDescription = "press J or X on controller to throw a Teleporter Snowball";
                    UIManager.Instance.hasSnowball();
                    hasSnowBallPower = true;
                }
                break;
        }
        if (shouldShowText == true)
        {
            UIManager.Instance.showPowerup(nameAndDescription);
        }

    }

    public void tryThrowSnowball()
    {
        if (canThrowSnowball == true)
        {
            if (hasSnowBallPower == true)
            {
                if (myMovement.isDashing == false)
                {
                    canThrowSnowball = false;
                    wantsToThrowSnowball = false;
                    throwSnowball();
                }
                else
                {
                    wantsToThrowSnowball = true;
                }
            }
        }
    }

    private void throwSnowball()
    {
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
        anim.ResetTrigger("ThrowSnowball");
        anim.SetTrigger("ThrowSnowball");
        UIManager.Instance.setSnowballInactive();
        hasStartedThrowingSnowball = true;
    }

    public void actuallyThrow()
    {
        AudioManager.instance.PlaySound("Throw");
        currentSnowball.gameObject.SetActive(true);
        hasStartedThrowingSnowball = false;
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
        AudioManager.instance.PlaySound("SnowballLand");
        AudioManager.instance.PlaySound("Dash");
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
        if (isDead == false)
        {
            if (canTakeDamage == true)
            {
                health -= amount;
                AudioManager.instance.PlaySound("PlayerHit");
                UIManager.Instance.updatePlayerHealthText(health, maxPlayerHealth);
                CameraShake.instance.addBigShake();
                //knockback  need to create function in playermovement script.
                myMovement.getKnockedBack(direction);
                AudioManager.instance.PlaySound("PlayerHurt");
                StartCoroutine(flashWait());
            }
            if (health <= 0)
            {
                //die
                isDead = true;
                anim.ResetTrigger("Die");
                anim.SetTrigger("Die");
                GameManager.Instance.StartGameOver();
                //StartCoroutine(waitForGameOver());
            }
        }
    }

    private IEnumerator flashWait()
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.color = new Vector4(1, 0, 0, 1);
        }

        yield return new WaitForSeconds(.2f);

        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.color = new Vector4(1, 1, 1, 1);
        }
    }

    private IEnumerator waitForGameOver()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.StartGameOver();
    }

    public void setPresents(int number)
    {
        numberOfPresentsCollected = number;
        UIManager.Instance.updatePresentText(numberOfPresentsCollected);
    }

    public void collectPresent()
    {
        numberOfPresentsCollected++;
        CameraShake.instance.addLittleShake();
        UIManager.Instance.updatePresentText(numberOfPresentsCollected);
        UIManager.Instance.shakePresentPanel();
        AudioManager.instance.PlaySound("PresentCollect");
    }

    public void KickRock()
    {
        myKicker.TryToKickRock();
    }

    public void spawnLandingDust()
    {
        myLandingDust.Play();
    }
}
