using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthHandler : MonoBehaviour
{
    BattleUnit battleUnit;

    [SerializeField] float maxHealth;
    float currentHealth;
    public bool IsDead { get; private set; }

    [SerializeField] AudioClip[] hitAudio;
    [SerializeField] AudioClip dieAudio;

    public event Action<float, float> OnHealthChanged;
    public event Action<BattleUnit> OnDied;

    void Awake()
    {
        battleUnit = GetComponent<BattleUnit>();

        currentHealth = maxHealth;
        IsDead = false;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
        else
        {
            AudioManager.Instance.PlaySFX(hitAudio[UnityEngine.Random.Range(0, hitAudio.Length)]);
            battleUnit.Animator.SetTrigger("Hit");
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Die()
    {
        IsDead = true;
        battleUnit.StatusEffectHandler.RemoveAllEffects();
        gameObject.layer = LayerMask.NameToLayer("Default");
        battleUnit.Animator.SetTrigger("Die");
        AudioManager.Instance.PlaySFX(dieAudio);
        OnDied?.Invoke(battleUnit);
    }
}
