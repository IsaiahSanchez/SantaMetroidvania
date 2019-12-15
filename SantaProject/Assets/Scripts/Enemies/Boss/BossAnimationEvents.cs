using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    [SerializeField] private BossMain boss;
    [SerializeField] private GameObject damageBox;

    public void readyToAttack()
    {
        boss.readyToAttackAgain();
    }

    public void tryEnableDamageBox()
    {
        if (boss.bossState != BossMain.bossStates.dead)
        {
            damageBox.SetActive(true);
        }
    }

    public void switchToActive()
    {
        boss.bossState = BossMain.bossStates.active;
    }

    public void shakeScreen()
    {
        CameraShake.instance.addBigShake();
        playThud();
    }

    public void playScream()
    {
        AudioManager.instance.PlaySound("BossShout");
    }

    private void playThud()
    {
        AudioManager.instance.PlaySound("BossSlam");
    }

}
