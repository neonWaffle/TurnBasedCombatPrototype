using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TargetType { ENEMY, ALLY, SELF }
public enum VFXSlotType { LOW, MEDIUM, HIGH }

public interface IAction
{
    void Begin(BattleUnit user, BattleUnit target);
    void Execute();
    void UpdateCooldown();
    int GetTurnsTilUseable();
    TargetType GetTargetType();

    public event Action OnCompleted;
}
