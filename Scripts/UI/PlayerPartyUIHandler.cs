using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class PlayerPartyUIHandler : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] CanvasGroup panel;
    [SerializeField] float fadeDuration = 1f;
    BattleUnit[] partyUnits;
    PlayerUnitPanel[] playerUnitPanels;

    void Awake()
    {
        playerUnitPanels = GetComponentsInChildren<PlayerUnitPanel>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);
    }

    void Start()
    {
        BattleManager.Instance.OnBattleStarted += EnableUI;
        BattleManager.Instance.OnBattleEnded += DisableUI;
    }

    void OnDestroy()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnBattleStarted -= EnableUI;
            BattleManager.Instance.OnBattleEnded -= DisableUI;
        }
    }

    void EnableUI()
    {
        panel.alpha = 0f;
        canvas.gameObject.SetActive(true);
        panel.DOFade(1f, fadeDuration);

        partyUnits = BattleManager.Instance.UnitOrder.Where(unit => !unit.Data.IsEnemy).ToArray();
        for (int i = 0; i < playerUnitPanels.Length; i++)
        {
            if (i < partyUnits.Length)
            {
                playerUnitPanels[i].gameObject.SetActive(true);
                playerUnitPanels[i].SetUnit(partyUnits[i]);
            }
            else
            {
                playerUnitPanels[i].gameObject.SetActive(false);
            }
        }

        canvas.gameObject.SetActive(true);
    }

    void DisableUI(BattleOutcome _)
    {
        panel.DOFade(0f, fadeDuration).OnComplete(() => canvas.gameObject.SetActive(false));
    }
}
