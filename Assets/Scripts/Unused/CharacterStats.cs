using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    public Stats damage; 


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // die in some way
        //method meant to be overwritten
    }
}
