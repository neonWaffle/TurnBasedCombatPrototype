using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(BattleUnit))]
public class EnemyUnitController : MonoBehaviour, IUnitController
{
    BattleUnit battleUnit;
    List<BattleUnit> playerUnits = new List<BattleUnit>();
    List<BattleUnit> enemyUnits = new List<BattleUnit>();
    [SerializeField, Tooltip("Use -1 for default attack")] int[] abilitiesToUse;
    int currentAbilityId = 0;

    void Awake()
    {
        battleUnit = GetComponent<BattleUnit>();
    }

    void Start()
    {
        var allUnits = FindObjectsOfType<BattleUnit>();

        playerUnits = allUnits.Where(unit => !unit.Data.IsEnemy).ToList();
        foreach (var unit in playerUnits)
        {
            unit.HealthHandler.OnDied += RemovePlayerUnit;
        }

        enemyUnits = allUnits.Where(unit => unit.Data.IsEnemy).ToList();
        foreach (var unit in enemyUnits)
        {
            unit.HealthHandler.OnDied += RemoveEnemyUnit;
        }
    }

    void RemovePlayerUnit(BattleUnit playerUnit)
    {
        playerUnit.HealthHandler.OnDied -= RemovePlayerUnit;
        playerUnits.Remove(playerUnit);
    }

    void RemoveEnemyUnit(BattleUnit enemyUnit)
    {
        enemyUnit.HealthHandler.OnDied -= RemoveEnemyUnit;
        enemyUnits.Remove(enemyUnit);
    }

    public void StartTurn()
    {
        SelectAction();
    }

    void SelectAction()
    {
        int abilityId = abilitiesToUse[currentAbilityId];
        if (abilityId == -1)
        {
            battleUnit.ActionHandler.SelectDefaultAttack();
        }
        else
        {
            battleUnit.ActionHandler.SelectAction(battleUnit.ActionHandler.Abilities[abilityId]);
        }

        var targetType = battleUnit.ActionHandler.SelectedAction.GetTargetType();
        if (targetType == TargetType.ENEMY)
        {
            battleUnit.ActionHandler.SelectTarget(playerUnits[Random.Range(0, playerUnits.Count)]);
        }
        else if (targetType == TargetType.ALLY)
        {
            battleUnit.ActionHandler.SelectTarget(enemyUnits[Random.Range(0, enemyUnits.Count)]);
        }

        currentAbilityId = currentAbilityId >= abilitiesToUse.Length - 1 ? 0 : currentAbilityId + 1;
    }
}
