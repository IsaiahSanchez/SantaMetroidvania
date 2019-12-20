using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : Door
{
    private bool hasBeenToldToOpen = false;


    private void Start()
    {
        if (GameDataManager.instance.bossIsAlive == true)
        {
            closeDoor();
        }
        else
        {
            openDoor();
        }
    }

    protected override void Update()
    {
        if (hasBeenToldToOpen == false)
        {
            if (GameDataManager.instance.bossIsAlive == false)
            {
                openDoor();
                hasBeenToldToOpen = true;
            }
        }
        base.Update();
    }
}
