using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType { FLAT, PERCENTAGE }

[CreateAssetMenu(fileName = "ModifierStatusEffect", menuName = "ScriptableObjects/StatusEffects/Modifier")]
public class ModifierStatusEffectSO : StatusEffectSO
{
    [SerializeField] StatType statType;
    public StatType StatType => statType;
    [SerializeField] float amount;
    public float Amount => amount;
    [SerializeField] ModifierType modifierType;
    public ModifierType ModifierType => modifierType;

    public override void Apply(BattleUnit target)
    {
        target.StatusNotifier.DisplayStatus(Message, MessageColour);
    }
}
