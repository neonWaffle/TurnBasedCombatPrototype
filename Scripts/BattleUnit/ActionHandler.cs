using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ActionHandler : MonoBehaviour
{
    BattleUnit battleUnit;
    IUnitController unitController;

    [SerializeField] AbilitySO defaultAttackSO;
    [SerializeField] AbilitySO[] abilities;
    Ability defaultAttack;
    public List<Ability> Abilities { get; private set; }

    public IAction SelectedAction { get; private set; }
    List<BattleUnit> potentialTargets = new List<BattleUnit>();

    public event Action OnActionStarted;
    public event Action OnActionFinished;

    void Awake()
    {
        battleUnit = GetComponent<BattleUnit>();
        unitController = GetComponent<IUnitController>();

        Abilities = new List<Ability>();
        foreach (var ability in abilities)
        {
            Abilities.Add(new Ability(ability));
        }

        defaultAttack = new Ability(defaultAttackSO);
    }

    public void SelectDefaultAttack()
    {
        SelectAction(defaultAttack);
    }

    public void StartTurn()
    {
        unitController.StartTurn();
    }

    public void SelectAction(IAction action)
    {
        if (SelectedAction != null)
        {
            SelectedAction.OnCompleted -= CompleteAction;
        }

        SelectedAction = action;
        SelectedAction.OnCompleted += CompleteAction;

        foreach (var potentialTarget in potentialTargets)
        {
            potentialTarget.MarkAsPotentialTarget(false);
        }

        switch (action.GetTargetType())
        {
            case TargetType.ENEMY:
                potentialTargets = BattleManager.Instance.UnitOrder.Where(unit => unit.Data.IsEnemy).ToList();
                foreach (var target in potentialTargets)
                {
                    target.MarkAsPotentialTarget(true);
                }
                break;
            case TargetType.ALLY:
                potentialTargets = BattleManager.Instance.UnitOrder.Where(unit => !unit.Data.IsEnemy).ToList();
                foreach (var target in potentialTargets)
                {
                    target.MarkAsPotentialTarget(true);
                }
                break;
            case TargetType.SELF:
                SelectTarget(battleUnit);
                break;
        }
    }

    public void SelectTarget(BattleUnit target)
    {
        foreach (var potentialTarget in potentialTargets)
        {
            potentialTarget.MarkAsPotentialTarget(false);
        }

        SelectedAction.Begin(battleUnit, target);
        OnActionStarted?.Invoke();
    }

    void CompleteAction()
    {
        SelectedAction = null;
        OnActionFinished?.Invoke();
    }

    //Animation event
    void ExecuteAction()
    {
        SelectedAction.Execute();
    }
}
