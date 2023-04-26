using System;
using System.Collections;
using System.Collections.Generic;

public class Ability : IAction
{
    BattleUnit user;
    BattleUnit target;

    int turnsTilUsable;
    bool usedThisTurn;

    public AbilitySO AbilitySO { get; private set; }

    public event Action OnCompleted;

    public Ability(AbilitySO abilitySO)
    {
        AbilitySO = abilitySO;
    }

    public TargetType GetTargetType()
    {
        return AbilitySO.TargetType;
    }

    public void Begin(BattleUnit user, BattleUnit target)
    {
        usedThisTurn = true;
        turnsTilUsable = AbilitySO.Cooldown;
        this.user = user;
        this.target = target;

        AbilitySO.OnCompleted += CompleteAction;
        AbilitySO.Begin(user);
    }

    public void Execute()
    {
        AbilitySO.Execute(user, target);
    }

    void CompleteAction()
    {
        AbilitySO.OnCompleted -= CompleteAction;
        OnCompleted?.Invoke();
    }

    public void UpdateCooldown()
    {
        if (usedThisTurn)
        {
            usedThisTurn = false;
        }
        else if (turnsTilUsable > 0)
        {
            turnsTilUsable--;
        }
    }

    public int GetTurnsTilUseable()
    {
        return turnsTilUsable;
    }
}
