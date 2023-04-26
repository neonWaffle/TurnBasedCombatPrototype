using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUnitPanel : MonoBehaviour
{
    BattleUnit unit;
    [SerializeField] Image healthBar;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI titleText;
    StatusEffectIcon[] statusEffectIcons;

    void Awake()
    {
        statusEffectIcons = GetComponentsInChildren<StatusEffectIcon>();
        foreach (var icon in statusEffectIcons)
        {
            icon.gameObject.SetActive(false);
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

    public void SetUnit(BattleUnit unit)
    {
        this.unit = unit;
        unit.HealthHandler.OnHealthChanged += UpdateHealth;
        unit.StatusEffectHandler.OnAdded += AddStatusEffect;
        unit.StatusEffectHandler.OnRemoved += RemoveStatusEffect;
        titleText.text = unit.Data.Title;
        portrait.sprite = unit.Data.Icon;
    }

    void UpdateHealth(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
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
}
