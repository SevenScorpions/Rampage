using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    int currentHealth;
    public int maxHealth = 100;
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth<=0)
        {
            currentHealth = 0;
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Enemy die");
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }
}
