using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ItemSO : ScriptableObject
{
    [SerializeField] string title;
    public string Title => title;
    [SerializeField] string description;
    public virtual string Description => description;
    [SerializeField] int cooldown;
    public virtual int Cooldown => cooldown;
    [SerializeField] TargetType targetType;
    public virtual TargetType TargetType => targetType;

    [SerializeField] GameObject vfx;
    [SerializeField] VFXSlotType vfxSlotType;

    public event Action OnCompleted;

    public abstract void Begin(BattleUnit user);

    public virtual void Execute(BattleUnit user, BattleUnit target)
    {
        if (vfx != null)
        {
            GameObject.Instantiate(vfx, target.Data.GetVFXSlot(vfxSlotType).position, vfx.transform.rotation);
        }
    }

    protected void Complete()
    {
        OnCompleted?.Invoke();
    }
}
