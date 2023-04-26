using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanel : MonoBehaviour
{
    [SerializeField] ActionButton[] abilityButtons;
    [SerializeField] ActionButton[] itemButtons;
    [SerializeField] CanvasGroup itemPanel;
    [SerializeField] CanvasGroup abilityPanel;
    [SerializeField] CanvasGroup fleeConfirmationPanel;
    Canvas canvas;

    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        fleeConfirmationPanel.gameObject.SetActive(false);
    }

    void Start()
    {
        DisableUI();
        BattleManager.Instance.OnTurnStarted += ToggleUI;
        BattleManager.Instance.OnUnitActionStarted += DisableUI;
    }

    void OnDestroy()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnTurnStarted -= ToggleUI;
            BattleManager.Instance.OnUnitActionStarted -= DisableUI;
        }
    }

    public void SelectDefaultAttack()
    {
        CloseActionPanels();
        BattleManager.Instance.CurrentUnit.ActionHandler.SelectDefaultAttack();
    }

    public void OpenItemPanel()
    {
        itemPanel.gameObject.SetActive(true);
        abilityPanel.gameObject.SetActive(false);
    }

    public void OpenAbilityPanel()
    {
        itemPanel.gameObject.SetActive(false);
        abilityPanel.gameObject.SetActive(true);
    }

    void CloseActionPanels()
    {
        itemPanel.gameObject.SetActive(false);
        abilityPanel.gameObject.SetActive(false);
    }

    public void Flee()
    {
        DisableUI();
        BattleManager.Instance.Flee();
    }

    void DisableUI()
    {
        canvas.gameObject.SetActive(false);
        CloseActionPanels();
    }

    void ToggleUI(bool isPlayerTurn)
    {
        if (!isPlayerTurn)
        {
            canvas.gameObject.SetActive(false);
            return;
        }

        CloseActionPanels();
        canvas.gameObject.SetActive(true);

        for (int i = 0; i < abilityButtons.Length; i++)
        {
            if (i < BattleManager.Instance.CurrentUnit.ActionHandler.Abilities.Count)
            {
                abilityButtons[i].SetInfo(BattleManager.Instance.CurrentUnit.ActionHandler.Abilities[i]);
                abilityButtons[i].gameObject.SetActive(true);
            }
            else
            {
                abilityButtons[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (i < BattleManager.Instance.CurrentUnit.Inventory.Items.Count)
            {
                itemButtons[i].SetInfo(BattleManager.Instance.CurrentUnit.Inventory.Items[i]);
                itemButtons[i].gameObject.SetActive(true);
            }
            else
            {
                itemButtons[i].gameObject.SetActive(false);
            }
        }
    }
}
