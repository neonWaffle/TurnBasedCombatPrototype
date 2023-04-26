using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AbilitySO : ScriptableObject
{
    [SerializeField] string title;
    public string Title => title;
    [SerializeField] string description;
    public string Description => description;
    [SerializeField] int cooldown;
    public int Cooldown => cooldown;
    [SerializeField] TargetType targetType;
    public TargetType TargetType => targetType;

    [SerializeField] protected StatusEffectSO[] statusEffects;
    [SerializeField] string animation;
    [SerializeField] AudioClip sfx;
    [SerializeField] AbilityParticle abilityParticle;
    [SerializeField] VFXSlotType vfxSlotType;
    [SerializeField] bool isRanged;
    public bool IsRanged => isRanged;

    public event Action OnCompleted;

    public void Begin(BattleUnit user)
    {
        user.Animator.SetTrigger(animation);

        if (sfx != null)
        {
            AudioManager.Instance.PlaySFX(sfx);
        }
    }

    public void Execute(BattleUnit user, BattleUnit target)
    {
        if (abilityParticle != null)
        {
            var spawnPos = IsRanged ? user.Data.GetVFXSlot(vfxSlotType).position : target.Data.GetVFXSlot(vfxSlotType).position;
            var particle = GameObject.Instantiate(abilityParticle, spawnPos, abilityParticle.transform.rotation);
            particle.Set(this, user, target);
        }

        if (!IsRanged)
        {
            Apply(user, target);
        }
    }

    public abstract void Apply(BattleUnit user, BattleUnit target);

    protected void Complete()
    {
        OnCompleted?.Invoke();
    }
}
