using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(StatusEffectHandler))]
[RequireComponent(typeof(ActionHandler))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(UnitStatusNotifier))]
[RequireComponent(typeof(HealthHandler))]
[RequireComponent(typeof(BattleUnitData))]
public class BattleUnit : MonoBehaviour
{
    public BattleUnitData Data { get; private set; }
    public Inventory Inventory { get; private set; }
    public ActionHandler ActionHandler { get; private set; }
    public HealthHandler HealthHandler { get; private set; }
    public StatusEffectHandler StatusEffectHandler { get; private set; }
    public UnitStatusNotifier StatusNotifier { get; private set; }
    public Animator Animator { get; private set; }

    Outline targetOutline;
    [SerializeField] GameObject targetIndicator;
    [SerializeField] GameObject isCurrentUnitIndicator;

    void Awake()
    {
        Data = GetComponent<BattleUnitData>();
        Inventory = GetComponent<Inventory>();
        HealthHandler = GetComponent<HealthHandler>();
        StatusEffectHandler = GetComponent<StatusEffectHandler>();
        ActionHandler = GetComponent<ActionHandler>();
        StatusNotifier = GetComponent<UnitStatusNotifier>();
        Animator = GetComponent<Animator>();
        targetOutline = GetComponent<Outline>();

        targetOutline.enabled = false;
        isCurrentUnitIndicator.SetActive(false);

        SetAsTarget(false);
    }

    public void PrepareForTurn()
    {
        isCurrentUnitIndicator.SetActive(true);

        foreach (var ability in ActionHandler.Abilities)
        {
            ability.UpdateCooldown();
        }

        foreach (var item in Inventory.Items)
        {
            item.UpdateCooldown();
        }

        StatusEffectHandler.ApplyStatusEffects();
    }

    public void MarkAsPotentialTarget(bool isPotentialTarget)
    {
        targetOutline.enabled = isPotentialTarget;
    }

    public void SetAsTarget(bool isTarget)
    {
        targetIndicator.SetActive(isTarget);
    }

    public void FinishTurn()
    {
        isCurrentUnitIndicator.SetActive(false);
        StatusEffectHandler.FinishTurn();
    }
}
