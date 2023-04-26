using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scroll", menuName = "ScriptableObjects/Scrolls")]
public class ScrollSO : ItemSO
{
    [SerializeField] AbilitySO abilitySO;
    public override int Cooldown => abilitySO.Cooldown;
    public override string Description => abilitySO.Description;
    public override TargetType TargetType => abilitySO.TargetType;

    public override void Begin(BattleUnit user)
    {
        abilitySO.Begin(user);
    }

    public override void Execute(BattleUnit user, BattleUnit target)
    {
        abilitySO.Execute(user, target);
        Complete();
    }
}
