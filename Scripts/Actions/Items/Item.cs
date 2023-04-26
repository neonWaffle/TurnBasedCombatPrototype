using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : IAction
{
    BattleUnit user;
    BattleUnit target;

    public ItemSO ItemSO { get; private set; }
    public int Quantity { get; private set; }

    int turnsTilUsable;
    bool usedThisTurn;

    public event Action OnCompleted;
    public event Action<Item> OnUsedUp;

    public Item(ItemSO itemSO, int quantity)
    {
        ItemSO = itemSO;
        Quantity = quantity;
    }

    public void IncreaseQuantity(int amount)
    {
        Quantity += amount;
    }

    public void DecreaseQuantity(int amount)
    {
        Quantity -= amount;
        if (Quantity <= 0)
        {
            OnUsedUp?.Invoke(this);
        }
    }

    public TargetType GetTargetType()
    {
        return ItemSO.TargetType;
    }

    public void Begin(BattleUnit user, BattleUnit target)
    {
        usedThisTurn = true;
        this.user = user;
        this.target = target;
        turnsTilUsable = ItemSO.Cooldown;

        ItemSO.OnCompleted += CompleteAction;
        ItemSO.Begin(user);

        DecreaseQuantity(1);
    }

    public void Execute()
    {
        ItemSO.Execute(user, target);
    }

    void CompleteAction()
    {
        ItemSO.OnCompleted -= CompleteAction;
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
