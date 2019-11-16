using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] Collider2D DamageWeakPoint;
    [SerializeField] Collider2D DamageTrigger;

    protected Rigidbody2D myBody;
    protected int hitPoints = 1;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    public virtual void damageEnemy()
    {
        hitPoints--;
        if (hitPoints <= 0)
        {
            die();
        }
    }

    protected virtual void die()
    {
        //disable hitboxes and play dying animation and then do nothing.
        AudioManager.instance.PlaySound("EnemyHurt");
    }

    public virtual void playerSeen()
    {
    
    }

    public virtual void playerLost()
    {

    }

}
