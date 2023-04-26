using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Stats
{
    [SerializeField] float attack;
    public float Attack => attack;
    [SerializeField] float defence;
    public float Defence => defence;
    [SerializeField] float initiative;
    public float Initiative => initiative;
}

public class BattleUnitData : MonoBehaviour
{
    [SerializeField] Stats stats;
    public Stats Stats => stats;

    [SerializeField] string title;
    public string Title => title;
    [SerializeField] Sprite icon;
    public Sprite Icon => icon;
    [SerializeField] bool isEnemy;
    public bool IsEnemy => isEnemy;

    [SerializeField] Transform vfxSlotLow;
    [SerializeField] Transform vfxSlotMedium;
    [SerializeField] Transform vfxSlotHigh;

    public Transform GetVFXSlot(VFXSlotType slotType)
    {
        switch (slotType)
        {
            case VFXSlotType.LOW:
                return vfxSlotLow;
            case VFXSlotType.MEDIUM:
                return vfxSlotMedium;
            case VFXSlotType.HIGH:
                return vfxSlotHigh;
            default:
                return null;
        }
    }
}
