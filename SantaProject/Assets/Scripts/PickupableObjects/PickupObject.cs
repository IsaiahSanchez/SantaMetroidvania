using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public int ObjID;
    public bool hasBeenCollected = false;

    private void Start()
    {
        if (hasBeenCollected == true)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        PickUpItem(target);
    }

    protected virtual void PickUpItem(Collider2D targetCollider)
    {
        hasBeenCollected = true;
        gameObject.SetActive(false);
    }

}
