using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackAbility", menuName = "ScriptableObjects/Abilities/Attack")]
public class AttackAbilitySO : AbilitySO
{
    [SerializeField] float baseDamage;

    public override void Apply(BattleUnit user, BattleUnit target)
    {
        float attack = user.StatusEffectHandler.CalculateStats(StatType.ATTACK);
        float defence = target.StatusEffectHandler.CalculateStats(StatType.DEFENCE);
        float amount = baseDamage + (attack / defence);

        if (amount > 0f)
        {
            target.HealthHandler.TakeDamage(amount);
            target.StatusNotifier.DisplayStatus(amount.ToString("0.0"));
            foreach (var statusEffect in statusEffects)
            {
                target.StatusEffectHandler.AddStatusEffect(statusEffect);
            }
        }
        else
        {
            target.StatusNotifier.DisplayStatus("Missed");
        }

        Complete();
    }
}
