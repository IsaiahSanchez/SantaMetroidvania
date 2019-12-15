using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentPickup : PickupObject
{
   
    protected override void PickUpItem(Collider2D targetCollider)
    {
        targetCollider.GetComponentInParent<PlayerMain>().collectPresent();
        AudioManager.instance.PlaySound("Collect");
        base.PickUpItem(targetCollider);
    }

}
