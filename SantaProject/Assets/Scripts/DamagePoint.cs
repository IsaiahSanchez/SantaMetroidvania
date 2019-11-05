using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePoint : MonoBehaviour
{
    [SerializeField] private Enemy myEnemy;

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.layer == 8)
        {
            myEnemy.damageEnemy();
        }
    }
}
