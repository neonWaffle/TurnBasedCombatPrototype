using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamagingStatusEffect", menuName = "ScriptableObjects/StatusEffects/Damaging")]
public class DamagingStatusEffectSO : StatusEffectSO
{
    [SerializeField] float amount;

    public override void Apply(BattleUnit target)
    {
        target.StatusNotifier.DisplayStatus(amount.ToString(), MessageColour);
        target.HealthHandler.TakeDamage(amount);
    }
}
