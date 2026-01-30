using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamageable
{
    public float Health = 100f;
    public bool IsInvincible;
    public virtual void Damage(float damage)
    {
        if(!IsInvincible)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
        }
    }

    public virtual void Die()
    {
        //death anim
        Destroy(gameObject);
    }
}
