using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private float DamageAmount;
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.layer == 9)
        {
            int tempDir = 0;
            if (target.transform.position.x < transform.position.x)
            {
                tempDir = -1;
            }
            else
            {
                tempDir = 1;
            }
            target.GetComponentInParent<PlayerMain>().TakeDamage(DamageAmount, tempDir);
        }
    }
}
