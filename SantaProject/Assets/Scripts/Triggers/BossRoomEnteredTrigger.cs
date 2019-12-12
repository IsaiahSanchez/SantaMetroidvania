using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomEnteredTrigger : Trigger
{
    [SerializeField] private BossMain boss;
    [SerializeField] private Door bossRoomDoor;
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
                hasBeenTriggered = true;
                bossRoomDoor.closeDoor();
            }
        }
    }
}
