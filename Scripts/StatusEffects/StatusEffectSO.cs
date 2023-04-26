using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffectSO : ScriptableObject
{
    [SerializeField] Sprite icon;
    public Sprite Icon => icon;
    [SerializeField] string description;
    public string Description => description;
    [SerializeField] int duration;
    public int Duration => duration;

    [SerializeField] AudioClip sfx;
    public AudioClip SFX => sfx;
    [SerializeField] string animation;
    public string Animation => animation;

    [SerializeField] string message;
    public string Message => message;
    [SerializeField] Color messageColour;
    public Color MessageColour => messageColour;

    [SerializeField] GameObject vfx;
    public GameObject VFX => vfx;
    [SerializeField] VFXSlotType vfxSlotType;
    public VFXSlotType VFXSlotType => vfxSlotType;
    [SerializeField] bool isVFXAttached;
    public bool IsVFXAttached => isVFXAttached;

    public abstract void Apply(BattleUnit target);
}
