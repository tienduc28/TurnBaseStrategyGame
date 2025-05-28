using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private int healthMax;
    public event EventHandler OnDead;
    public event EventHandler OnDameged;

    private void Awake()
    {
        healthMax = health; // Store the initial max health
    }
    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0)
        {
            health = 0;
        }

        OnDameged?.Invoke(this, EventArgs.Empty); // Notify subscribers that damage has occurred

        if (health == 0)
        {
            Die();
        }

        Debug.Log("Health left: " + health);
    }

    public void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax; // Return normalized health value
    }
}
