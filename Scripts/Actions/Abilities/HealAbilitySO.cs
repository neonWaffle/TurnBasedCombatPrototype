using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealAbility", menuName = "ScriptableObjects/Abilities/Heal")]
public class HealAbilitySO : AbilitySO
{
    [SerializeField] float amount;

    public override void Apply(BattleUnit user, BattleUnit target)
    {
        target.StatusNotifier.DisplayStatus(amount.ToString());
        target.HealthHandler.Heal(amount);

        foreach (var statusEffect in statusEffects)
        {
            target.StatusEffectHandler.AddStatusEffect(statusEffect);
        }

        Complete();
    }
}
