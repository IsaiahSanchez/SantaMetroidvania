using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    protected Rigidbody2D myBody;
    protected int hitPoints = 1;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    public void damageEnemy()
    {
        hitPoints--;
        if (hitPoints <= 0)
        {
            //disable hitboxes and play dying animation and then do nothing.
        }
    }

}
