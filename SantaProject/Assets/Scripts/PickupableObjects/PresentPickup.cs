using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentPickup : PickupObject
{

    protected override void PickUpItem(Collider2D targetCollider)
    {
        targetCollider.GetComponentInParent<PlayerMain>().numberOfPresentsCollected++;
        Debug.Log(targetCollider.GetComponentInParent<PlayerMain>().numberOfPresentsCollected);


        base.PickUpItem(targetCollider);
    }

}
