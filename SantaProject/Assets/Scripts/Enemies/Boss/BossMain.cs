using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMain : Enemy
{
    public static BossMain instance;

    [SerializeField] private List<string> animations = new List<string>();
    [SerializeField] private Animator anim;
    [SerializeField] private float timeInBetweenRounds;
    [SerializeField] private GameObject myDamageBox;
    [SerializeField] private GameObject bloodParticle;
    [SerializeField] private Door bossRoomDoor;
    [SerializeField] private GameObject headHurtBox;

    public enum bossStates {offScreen, bossRoomEntered, active, dead}
    public bossStates bossState = bossStates.offScreen;
    private bool isAttacking = false;

    private void Awake()
    {
        instance = this;
    }

    public void setBossAliveState(bool isAlive)
    {
        if (isAlive)
        {
            bossState = bossStates.offScreen;
        }
        else
        {
            bossState = bossStates.dead;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (bossState)
        {
            case bossStates.offScreen:

                break;
            case bossStates.bossRoomEntered:
                //play animation and wait to be triggerd active
                if (isAttacking == false)
                {
                    anim.ResetTrigger("RoomEntered");
                    anim.SetTrigger("RoomEntered");
                    isAttacking = true;
                }
                break;
            case bossStates.active:
                if (isAttacking == false)
                {
                    StartCoroutine(pauseBeforeAttack());
                    isAttacking = true;
                }
                break;
            case bossStates.dead:
                //play death animation
                break;
        }
    }

    public void readyToAttackAgain()
    {
        isAttacking = false;
    }

    public IEnumerator pauseBeforeAttack()
    {
        yield return new WaitForSeconds(timeInBetweenRounds);
        //randomly generate attack to do.
        if (bossState == bossStates.active)
        {
            myDamageBox.SetActive(false);
            headHurtBox.SetActive(true);
            int rand = Random.Range(0, animations.Count);
            anim.ResetTrigger(animations[rand]);
            anim.SetTrigger(animations[rand]);
        }
    }

    public override void damageEnemy()
    {
        AudioManager.instance.PlaySound("BossShout");
        CameraShake.instance.addLittleShake();
        Instantiate(bloodParticle, new Vector2(transform.position.x, transform.position.y+2f), Quaternion.identity);
        myDamageBox.SetActive(false);
        base.damageEnemy();
    }

    protected override void die()
    {
        CameraShake.instance.addBigShake();
        bossState = bossStates.dead;
        GameDataManager.instance.bossIsAlive = false;
        bossRoomDoor.openDoor();
        base.die();
    }
}
