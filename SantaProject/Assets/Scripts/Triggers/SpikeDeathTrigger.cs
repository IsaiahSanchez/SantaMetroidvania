using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDeathTrigger : Trigger
{
    protected override void Triggered()
    {
        GameManager.Instance.mainPlayer.canTakeDamage = true;
        GameManager.Instance.mainPlayer.TakeDamage(300f, 1);
    }
}
