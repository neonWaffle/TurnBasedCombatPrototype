using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum BattleOutcome { PLAYER_WON, PLAYER_LOST, PLAYER_FLED }

[RequireComponent(typeof(BattleAnimationPanel))]
public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public BattleUnit CurrentUnit { get; private set; }
    public List<BattleUnit> UnitOrder { get; private set; } = new List<BattleUnit>();
    int currentUnitId;
    int enemyUnits;
    int playerUnits;
    bool isBattleOver;

    BattleAnimationPanel animationPanel;
    [SerializeField] float breakDuration = 1.5f;

    [SerializeField] AudioClip victoryAudio;
    [SerializeField] AudioClip lossAudio;

    public event Action OnBattleStarted;
    public event Action<BattleOutcome> OnBattleEnded;
    public event Action<bool> OnTurnStarted;
    public event Action OnUnitActionStarted;
    public event Action<int> OnUnitOrderUpdated;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        animationPanel = GetComponent<BattleAnimationPanel>();
        animationPanel.OnAnimationCompleted += StartBattle;
    }

    void OnDestroy()
    {
        if (animationPanel != null)
        {
            animationPanel.OnAnimationCompleted -= StartBattle;
        }
    }

    void Start()
    {
        animationPanel.PlayBattleStartAnimation();
    }

    public void StartBattle()
    {
        isBattleOver = false;
        currentUnitId = -1;
        animationPanel.OnAnimationCompleted -= StartBattle;

        var battleUnits = FindObjectsOfType<BattleUnit>();
        foreach (var unit in battleUnits)
        {
            unit.HealthHandler.OnDied += RemoveUnit;
            if (unit.Data.IsEnemy)
            {
                enemyUnits++;
            }
            else
            {
                playerUnits++;
            }
        }

        SetAttackOrder(battleUnits);
        PrepareUnits();
        OnBattleStarted?.Invoke();
    }

    void SetAttackOrder(BattleUnit[] battleUnits)
    {
        var sortedUnits = battleUnits.OrderByDescending(unit => unit.Data.Stats.Initiative);
        foreach (var unit in sortedUnits)
        {
            UnitOrder.Add(unit);
        }
    }

    void PrepareUnits()
    {
        currentUnitId = currentUnitId >= UnitOrder.Count - 1 ? 0 : currentUnitId + 1;
        CurrentUnit = UnitOrder[currentUnitId];
        CurrentUnit.StatusEffectHandler.OnApplied += StartTurn;
        CurrentUnit.PrepareForTurn();
        OnUnitOrderUpdated?.Invoke(currentUnitId);
    }

    void StartTurn()
    {
        CurrentUnit.StatusEffectHandler.OnApplied -= StartTurn;

        //Checking in case the last enemy/player unit died because of status effects
        if (isBattleOver)
            return;

        if (!CurrentUnit.HealthHandler.IsDead && !CurrentUnit.StatusEffectHandler.IsIncapacitated)
        {
            StartCoroutine(StartTurnWithDelay());
        }
        else
        {
            StartCoroutine(FinishTurn());
        }
    }

    public void Flee()
    {
        StartCoroutine(EndBattle(BattleOutcome.PLAYER_FLED));
    }

    IEnumerator StartTurnWithDelay()
    {
        yield return new WaitForSeconds(breakDuration);
        CurrentUnit.ActionHandler.OnActionStarted += StartUnitAction;
        CurrentUnit.ActionHandler.StartTurn();
        OnTurnStarted?.Invoke(!CurrentUnit.Data.IsEnemy);
    }

    void StartUnitAction()
    {
        CurrentUnit.ActionHandler.OnActionStarted -= StartUnitAction;
        CurrentUnit.ActionHandler.OnActionFinished += FinishUnitAction;
        OnUnitActionStarted?.Invoke();
    }

    void FinishUnitAction()
    {
        CurrentUnit.ActionHandler.OnActionFinished -= FinishUnitAction;
        StartCoroutine(FinishTurn());
    }

    IEnumerator FinishTurn()
    {
        yield return new WaitForSeconds(breakDuration);
        CurrentUnit.FinishTurn();
        if (!isBattleOver)
        {
            PrepareUnits();
        }
    }

    void RemoveUnit(BattleUnit deadUnit)
    {
        deadUnit.HealthHandler.OnDied -= RemoveUnit;
        int deadUnitId = UnitOrder.FindIndex(unit => unit == deadUnit);

        if (deadUnitId <= currentUnitId)
        {
            currentUnitId--;
        }
        deadUnit.SetAsTarget(false);

        UnitOrder.Remove(deadUnit);

        if (deadUnit.Data.IsEnemy)
        {
            enemyUnits--;
            if (enemyUnits == 0)
            {
                StartCoroutine(EndBattle(BattleOutcome.PLAYER_WON));
            }
        }
        else
        {
            playerUnits--;
            if (playerUnits == 0)
            {
                StartCoroutine(EndBattle(BattleOutcome.PLAYER_LOST));
            }
        }

        OnUnitOrderUpdated?.Invoke(currentUnitId);
    }

    IEnumerator EndBattle(BattleOutcome battleOutcome)
    {
        isBattleOver = true;
        yield return new WaitForSeconds(breakDuration);

        if (battleOutcome == BattleOutcome.PLAYER_WON)
        {
            AudioManager.Instance.PlayMusic(victoryAudio, false);
        }
        else
        {
            AudioManager.Instance.PlayMusic(lossAudio, false);
        }

        OnBattleEnded?.Invoke(battleOutcome);
    }
}
