using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum StatType { ATTACK, DEFENCE, INITIATIVE }

public class StatusEffectHandler : MonoBehaviour
{
    List<StatusEffect> statusEffects = new List<StatusEffect>();
    public bool IsIncapacitated { get; private set; }
    [SerializeField] float vfxScale = 1f;
    public Vector3 VFXScale => vfxScale * Vector3.one;

    BattleUnit battleUnit;

    public event Action<StatusEffect> OnAdded;
    public event Action<StatusEffect> OnRemoved;
    public event Action OnApplied;

    void Awake()
    {
        battleUnit = GetComponent<BattleUnit>();
    }

    public void AddStatusEffect(StatusEffectSO statusEffectSO)
    {
        if (battleUnit.HealthHandler.IsDead)
            return;

        var statusEffect = statusEffects.FirstOrDefault(statusEffect => statusEffect.StatusEffectSO == statusEffectSO);
        if (statusEffect != null)
        {
            OnRemoved?.Invoke(statusEffect); //Gets readded later
            statusEffect.Renew();
        }
        else
        {
            statusEffect = new StatusEffect(statusEffectSO, battleUnit);
            statusEffects.Add(statusEffect);
        }

        OnAdded?.Invoke(statusEffect);
    }

    public void Incapacitate()
    {
        IsIncapacitated = true;
    }

    public void RemoveAllEffects()
    {
        foreach (var statusEffect in statusEffects)
        {
            OnRemoved?.Invoke(statusEffect);
            statusEffect.Remove();
        }
        statusEffects.Clear();
    }

    public float CalculateStats(StatType statType)
    {
        float finalValue = statType == StatType.ATTACK ? battleUnit.Data.Stats.Attack : battleUnit.Data.Stats.Defence;

        foreach (var effect in statusEffects)
        {
            if (effect.StatusEffectSO is ModifierStatusEffectSO modifierEffectSO && modifierEffectSO.StatType == statType)
            {
                switch (modifierEffectSO.ModifierType)
                {
                    case ModifierType.FLAT:
                        finalValue += modifierEffectSO.Amount;
                        break;
                    case ModifierType.PERCENTAGE:
                        if (statType == StatType.ATTACK)
                        {
                            finalValue += battleUnit.Data.Stats.Attack * modifierEffectSO.Amount;
                        }
                        else if (statType == StatType.DEFENCE)
                        {
                            finalValue += battleUnit.Data.Stats.Defence * modifierEffectSO.Amount;
                        }
                        break;
                }
            }
        }

        return finalValue;
    }

    public void ApplyStatusEffects()
    {
        IsIncapacitated = false;

        for (int i = 0; i < statusEffects.Count; i++)
        { 
            statusEffects[i].Apply();
        }

        OnApplied?.Invoke();    
    }

    public void FinishTurn()
    {
        for (int i = statusEffects.Count - 1; i >= 0; i--)
        {
            if (!statusEffects[i].AdvanceTurn())
            {
                OnRemoved?.Invoke(statusEffects[i]);
                statusEffects[i].Remove();
                statusEffects.RemoveAt(i);
            }
        }
    }
}
