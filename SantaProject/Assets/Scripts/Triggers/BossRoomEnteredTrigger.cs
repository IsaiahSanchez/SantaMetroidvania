using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomEnteredTrigger : Trigger
{
    [SerializeField] private BossMain boss;
    [SerializeField] private Door bossRoomDoor;
    [SerializeField] private GameObject stoppingWall;
    //door to close after player

    private bool hasBeenTriggered = false;

    protected override void Triggered()
    {
        if (hasBeenTriggered == false)
        {
            //set boss wake up
            if (boss.bossState != BossMain.bossStates.dead)
            {
                boss.bossState = BossMain.bossStates.bossRoomEntered;
                AudioManager.instance.PlaySound("BossShout");
                hasBeenTriggered = true;
                bossRoomDoor.closeDoor();
                UIManager.Instance.enableBossHealthBar();
                StartCoroutine(waitToDisableHitbox());
            }
        }
    }

    private IEnumerator waitToDisableHitbox()
    {
        stoppingWall.SetActive(true);
        yield return new WaitForSeconds(1f);
        stoppingWall.SetActive(false);
    }
}
