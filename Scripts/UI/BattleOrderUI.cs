using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleOrderUI : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] CanvasGroup panel;
    BattleUnitPortrait[] unitPortraits;
    [SerializeField] Image turnDivider;
    [SerializeField] float fadeDuration = 1f;

    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        unitPortraits = GetComponentsInChildren<BattleUnitPortrait>();
        canvas.gameObject.SetActive(false);
    }

    void Start()
    {
        BattleManager.Instance.OnUnitOrderUpdated += UpdateTurnOrder;
        BattleManager.Instance.OnBattleStarted += EnableUI;
        BattleManager.Instance.OnBattleEnded += DisableUI;
    }

    void OnDestroy()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnUnitOrderUpdated -= UpdateTurnOrder;
            BattleManager.Instance.OnBattleStarted -= EnableUI;
            BattleManager.Instance.OnBattleEnded -= DisableUI;
        }
    }

    void DisableUI(BattleOutcome _)
    {
        panel.DOFade(0f, fadeDuration).OnComplete(() => canvas.gameObject.SetActive(false));
    }

    void EnableUI()
    {
        panel.alpha = 0f;
        canvas.gameObject.SetActive(true);
        panel.DOFade(1f, fadeDuration);
    }

    void UpdateTurnOrder(int currentUnitId)
    {
        turnDivider.gameObject.SetActive(false);
        currentUnitId = Mathf.Max(0, currentUnitId);
        int unitId = currentUnitId;
        bool isNextTurn = false;

        foreach (var portrait in unitPortraits)
        {
            portrait.Set(BattleManager.Instance.UnitOrder[unitId], unitId == currentUnitId && !isNextTurn);

            if (unitId == BattleManager.Instance.UnitOrder.Count - currentUnitId - 1 && !isNextTurn)
            {
                isNextTurn = true;
                turnDivider.transform.SetSiblingIndex(unitId + 1);
                turnDivider.gameObject.SetActive(true);
            }

            unitId = unitId >= BattleManager.Instance.UnitOrder.Count - 1 ? 0 : unitId + 1;
        }
    }
}
