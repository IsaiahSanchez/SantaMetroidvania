using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickup : PickupObject
{
    private enum powerup {doubleJump, Dash, Snowball}
    [SerializeField] powerup chosenPowerUp = powerup.doubleJump;
    private int indexOfPowerup = 0;

    protected override void PickUpItem(Collider2D targetCollider)
    {
        targetCollider.GetComponentInParent<PlayerMain>().givePower((int)chosenPowerUp);
        base.PickUpItem(targetCollider);
    }

}
