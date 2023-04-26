using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnitUI : MonoBehaviour
{
    BattleUnit unit;
    [SerializeField] Image healthBar;
    [SerializeField] float targetSize = 0.15f;
    StatusEffectIcon[] statusEffectIcons;

    void Awake()
    {
        statusEffectIcons = GetComponentsInChildren<StatusEffectIcon>();
        foreach (var icon in statusEffectIcons)
        {
            icon.gameObject.SetActive(false);
        }

        unit = transform.root.GetComponent<BattleUnit>();
        unit.HealthHandler.OnHealthChanged += UpdateHealth;
        unit.StatusEffectHandler.OnAdded += AddStatusEffect;
        unit.StatusEffectHandler.OnRemoved += RemoveStatusEffect;
    }

    void Start()
    {
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        transform.localScale = distance * targetSize * Vector3.one;
    }

    void OnDestroy()
    {
        if (unit != null)
        {
            unit.HealthHandler.OnHealthChanged -= UpdateHealth;
            unit.StatusEffectHandler.OnAdded -= AddStatusEffect;
            unit.StatusEffectHandler.OnRemoved -= RemoveStatusEffect;
        }
    }

    void UpdateHealth(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
        if (currentHealth <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    void AddStatusEffect(StatusEffect statusEffect)
    {
        foreach (var icon in statusEffectIcons)
        {
            if (icon.StatusEffect == null)
            {
                icon.AddStatusEffect(statusEffect);
                break;
            }
        }
    }

    void RemoveStatusEffect(StatusEffect statusEffect)
    {
        foreach (var icon in statusEffectIcons)
        {
            if (icon.StatusEffect == statusEffect)
            {
                icon.RemoveStatusEffect();
                break;
            }
        }
    }
}
