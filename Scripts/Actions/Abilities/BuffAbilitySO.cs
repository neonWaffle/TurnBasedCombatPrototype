using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffAbility", menuName = "ScriptableObjects/Abilities/Buff")]
public class BuffAbilitySO : AbilitySO
{
    public override void Apply(BattleUnit user, BattleUnit target)
    {
        foreach (var statusEffect in statusEffects)
        {
            target.StatusEffectHandler.AddStatusEffect(statusEffect);
        }

        Complete();
    }
}
