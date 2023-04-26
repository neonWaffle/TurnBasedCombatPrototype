using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatusEffect
{
    public StatusEffectSO StatusEffectSO { get; private set; }
    int remainingTurns;
    GameObject vfx;
    BattleUnit target;
    bool justAdded;

    public event Action<int> OnRemainingTurnsUpdated;

    public StatusEffect(StatusEffectSO statusEffectSO, BattleUnit target)
    {
        justAdded = true;
        this.target = target;
        StatusEffectSO = statusEffectSO;
        remainingTurns = statusEffectSO.Duration;

        if (StatusEffectSO.VFX != null)
        {
            var slot = target.Data.GetVFXSlot(statusEffectSO.VFXSlotType);

            if (statusEffectSO.IsVFXAttached)
            {
                vfx = GameObject.Instantiate(StatusEffectSO.VFX, slot);
            }
            else
            {
                vfx = GameObject.Instantiate(StatusEffectSO.VFX, slot.position, StatusEffectSO.VFX.transform.rotation);
            }

            vfx.transform.localScale = target.StatusEffectHandler.VFXScale;
        }

        if (!string.IsNullOrWhiteSpace(StatusEffectSO.Message))
        {
            target.StatusNotifier.DisplayStatus(StatusEffectSO.Message, statusEffectSO.MessageColour);
        }

        if (!string.IsNullOrWhiteSpace(statusEffectSO.Animation))
        {
            target.Animator.SetBool(statusEffectSO.Animation, true);
        }
    }

    public void Renew()
    {
        justAdded = true;
        remainingTurns = StatusEffectSO.Duration;

        if (!string.IsNullOrWhiteSpace(StatusEffectSO.Message))
        {
            target.StatusNotifier.DisplayStatus(StatusEffectSO.Message, StatusEffectSO.MessageColour);
        }
    }

    public void Apply()
    {
        if (StatusEffectSO.SFX != null)
        {
            AudioManager.Instance.PlaySFX(StatusEffectSO.SFX);
        }

        justAdded = false;
        StatusEffectSO.Apply(target);
    }

    public bool AdvanceTurn()
    {
        //Don't want to update status effects that have only been added this turn
        if (!justAdded)
        {
            remainingTurns--;
            OnRemainingTurnsUpdated?.Invoke(remainingTurns);
        }

        return remainingTurns > 0;
    }

    public void Remove()
    {
        if (vfx != null)
        {
            GameObject.Destroy(vfx);
        }

        if (!string.IsNullOrWhiteSpace(StatusEffectSO.Animation))
        {
            target.Animator.SetBool(StatusEffectSO.Animation, false);
        }
    }
}
