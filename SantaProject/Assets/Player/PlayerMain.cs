using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] private Transform snowballThrowLocation;
    [SerializeField] private GameObject SnowBallPrefab;
    [SerializeField] private float SnowBallThrowSpeed;
    [SerializeField] private float timeAfterTeleportToEnableThrowing;

    private bool canThrowSnowball = true;
    private Coroutine mainWaitCoroutine;
    private SnowBall currentSnowball;
    private float health = 100f;

    private PlayerMovement myMovement;

    // Start is called before the first frame update
    void Start()
    {
        myMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canThrowSnowball == true)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                throwSnowball();
            }
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
        }
        else
        {
            currentSnowball.gameObject.SetActive(true);
            currentSnowball.transform.position = snowballThrowLocation.position;
            currentSnowball.transform.parent = snowballThrowLocation;
        }
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
        Debug.Log(health);
        //knockback  need to create function in playermovement script.
        myMovement.getKnockedBack(direction);
        if (health <= 0)
        {
            //die
        }
    }

}
