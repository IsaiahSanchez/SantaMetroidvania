using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] Collider2D DamageWeakPoint;
    [SerializeField] Collider2D DamageTrigger;
    [SerializeField] GameObject DeathParticles;

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
            CameraShake.instance.addLittleShake();
            die();
        }
    }

    protected virtual void die()
    {
        //disable hitboxes and play dying animation and then do nothing.
        Instantiate(DeathParticles, new Vector2(transform.position.x, transform.position.y -.25f), Quaternion.identity);
        AudioManager.instance.PlaySound("EnemyDeath");
    }

    public virtual void playerSeen()
    {
    
    }

    public virtual void playerLost()
    {

    }

}
