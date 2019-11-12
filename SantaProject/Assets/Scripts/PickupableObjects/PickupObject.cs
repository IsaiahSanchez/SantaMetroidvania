using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D target)
    {
        PickUpItem(target);
    }

    protected virtual void PickUpItem(Collider2D targetCollider)
    {
        Destroy(gameObject);
    }

}
