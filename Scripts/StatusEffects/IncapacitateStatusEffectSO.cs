using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncapacitateStatusEffect", menuName = "ScriptableObjects/StatusEffects/Incapacitate")]
public class IncapacitateStatusEffectSO : StatusEffectSO
{  
    public override void Apply(BattleUnit target)
    {
        target.StatusNotifier.DisplayStatus(Message, MessageColour);
        target.StatusEffectHandler.Incapacitate();
    }
}
