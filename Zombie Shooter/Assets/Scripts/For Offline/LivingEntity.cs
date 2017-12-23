using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{

    public float startingHealth;
    protected float health;
    public RectTransform healthebar;
    protected bool dead;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
    }

	public virtual void TakeHit(float damage, Vector3 hitPoint,Vector3 hitDirection)
    {
        // Do some stuff here with hit var
        TakeDamage(damage);
    }

	public virtual void TakeDamage(float damage)
    {
       
        if (health <= 0 && !dead)
        {
            Die();
        }
		health -= damage;
		healthebar.sizeDelta = new Vector2(health * 2, healthebar.sizeDelta.y);

    }

    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
            OnDeath();
        GameObject.Destroy(gameObject);
    }
}
