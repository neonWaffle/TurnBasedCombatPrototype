using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BattleUnit))]
public class PlayerUnitController : MonoBehaviour, IUnitController
{
    BattleUnit battleUnit;
    BattleUnit hoverTarget;
    [SerializeField] LayerMask unitLayerMask;
    bool hasTarget;

    void Awake()
    {
        battleUnit = GetComponent<BattleUnit>();
    }

    public void StartTurn()
    {
        StartCoroutine(SelectTarget());
    }

    IEnumerator SelectTarget()
    {
        hasTarget = false;

        while (!hasTarget)
        {
            if (battleUnit.ActionHandler.SelectedAction != null)
            {
                if (Input.GetButtonDown("Fire"))
                {
                    SetTarget();
                }
                else
                {
                    ShowIfValidTarget();
                }
            }
            yield return null;
        }
    }

    void SetTarget()
    {
        if (hoverTarget != null)
        {
            hasTarget = true;
            battleUnit.ActionHandler.SelectTarget(hoverTarget);
            RemoveCurrentHoverTarget();
        }
    }

    bool IsValidTarget(RaycastHit hit)
    {
        var targetType = battleUnit.ActionHandler.SelectedAction.GetTargetType();
        return targetType == TargetType.ALLY && hit.transform.CompareTag("Ally")
                || (targetType == TargetType.ENEMY && hit.transform.CompareTag("Enemy"));
    }

    void ShowIfValidTarget()
    {
        if (!EventSystem.current.IsPointerOverGameObject()
            && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, unitLayerMask)
            && IsValidTarget(hit))
        {
            if (hoverTarget == null || hoverTarget.gameObject != hit.transform.gameObject)
            {
                SetHoverTarget(hit.transform.GetComponent<BattleUnit>());
            }
        }
        else
        {
            RemoveCurrentHoverTarget();
        }
    }

    void SetHoverTarget(BattleUnit unit)
    {
        if (hoverTarget != null)
        {
            hoverTarget.SetAsTarget(false);
        }
        hoverTarget = unit;
        hoverTarget.SetAsTarget(true);
    }

    void RemoveCurrentHoverTarget()
    {
        if (hoverTarget != null)
        {
            hoverTarget.SetAsTarget(false);
            hoverTarget = null;
        }
    }
}
