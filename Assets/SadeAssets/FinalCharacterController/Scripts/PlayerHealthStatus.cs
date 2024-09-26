using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;       // Health player when starting game

    private float currentHealth;

    public HealthBarUI healthBar;
    private void Start()
    {
        currentHealth = maxHealth;

        healthBar.SetSliderMax(maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.SetSlider(currentHealth);
    }

    private void Update()
    {
        // Key k does damage to player
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10f);
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("You died!");

        // Play death animation "Fall"
        // Show death screen
    }
}
