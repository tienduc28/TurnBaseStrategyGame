using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health = 100;
    public event EventHandler OnDead;

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0)
        {
            health = 0;
            Die();
        }

        Debug.Log("Health left: " + health);
    }

    public void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
