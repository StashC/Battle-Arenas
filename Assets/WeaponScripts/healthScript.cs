using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class healthScript : MonoBehaviour
{
    public int _team;

    [SerializeField] private int _maxHealth = 50;
    private int currentHealth;
    [SerializeField] private UnityEvent onDeathEvent;

    private bool isInvulnerable;

    void Start()
    {
        currentHealth = _maxHealth;
        isInvulnerable = false;
    }

    public void DealDamage(int damage)
    {
        if(isInvulnerable) return;
        currentHealth -= damage;

        if (currentHealth <= 0) onDeathEvent.Invoke();
        //Debug.LogError("My health is: " + currentHealth);

    }

    public void Heal(int healAmount)
    {
        Debug.Log("healing for " + healAmount);
        currentHealth += healAmount;
        if (currentHealth > _maxHealth) currentHealth = _maxHealth;
    }

    public void SetInvulnerable(float duration) {
        Debug.Log("Player is Invulnverable");
        isInvulnerable = false;
        funcTimer.Create(SetVulnerable, duration);
    }

    private void SetVulnerable() {
        Debug.Log("Player is no longer Invulnverable");
        isInvulnerable = true;
    }
}
