using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionType { GENERIC, HEALTH }

[CreateAssetMenu(fileName = "Potion", menuName = "ScriptableObjects/Potions")]
public class PotionSO : ItemSO
{
    [SerializeField] float amount;
    [SerializeField] PotionType potionType;
    [SerializeField] StatusEffectSO[] statusEffects;

    public override void Begin(BattleUnit user)
    {
        user.Animator.SetTrigger("UsePotion");
    }

    public override void Execute(BattleUnit user, BattleUnit target)
    {
        base.Execute(user, target);

        switch (potionType)
        {
            case PotionType.HEALTH:
                target.HealthHandler.Heal(amount);
                break;
            case PotionType.GENERIC: //Some potions are only to going to give status effects instead of affecting the target directly
                break;
        }

        foreach (var statusEffect in statusEffects)
        {
            target.StatusEffectHandler.AddStatusEffect(statusEffect);
        }

        Complete();
    }
}
