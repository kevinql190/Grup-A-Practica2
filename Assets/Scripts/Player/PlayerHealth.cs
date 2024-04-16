using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Life")]
    public int maxHealth;
    [SerializeField] private Cooldown damageCooldown;
    public event Action<int> OnHealthChanged;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = value; OnHealthChanged?.Invoke(value); }
    }
    private int _currentHealth;
    private PlayerMovement _playerMovement;
    private void Start()
    {
        CurrentHealth = maxHealth;
        _playerMovement = GetComponent<PlayerMovement>();
    }
    public IEnumerator ResetHearts()
    {
        while (CurrentHealth < maxHealth)
        {
            CurrentHealth = maxHealth;
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void TakeDamage(int damage)
    {
        if (_playerMovement.isDashing || damageCooldown.IsCoolingDown) { Debug.Log("Invulnerable"); return; }
        int result = CurrentHealth += damage;
        if (result > maxHealth)
        {
            CurrentHealth = maxHealth;
        }
        else if (result < 1)
        {
            Die();
        }
        else
        {
            CurrentHealth = result;
        }
        damageCooldown.StartCooldown();
    }

    public void Die()
    {
        Debug.Log("You Dead");
    }
}

