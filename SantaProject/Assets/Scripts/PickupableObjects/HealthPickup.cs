using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : PickupObject
{
    protected override void PickUpItem(Collider2D targetCollider)
    {
        targetCollider.GetComponentInParent<PlayerMain>().TakeDamage(-50, 0);
        base.PickUpItem(targetCollider);
    }
}
