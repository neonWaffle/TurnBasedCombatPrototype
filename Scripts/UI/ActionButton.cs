using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(InfoDisplay))]
public class ActionButton : MonoBehaviour
{
    IAction action;
    [SerializeField] TextMeshProUGUI titleText;
    InfoDisplay infoDisplay;
    Button button;

    void Awake()
    {
        infoDisplay = GetComponent<InfoDisplay>();
        button = GetComponent<Button>();
    }

    public void SetInfo(IAction action)
    {
        this.action = action;
        int turnsTilUseable = action.GetTurnsTilUseable();
        button.interactable = turnsTilUseable == 0;

        string cooldownText = turnsTilUseable == 0
            ? string.Empty : turnsTilUseable == 1
            ? $"Cooldown over in {turnsTilUseable} turn." : $"Cooldown over in {turnsTilUseable} turns.";

        if (action is Ability ability)
        {
            titleText.text = ability.AbilitySO.Title;
            infoDisplay.Set(ability.AbilitySO.Title, $"{ability.AbilitySO.Description}\n\n{cooldownText}");
        }
        else if (action is Item item)
        {
            titleText.text = $"{item.ItemSO.Title}: {item.Quantity}";
            infoDisplay.Set(item.ItemSO.Title, $"{item.ItemSO.Description}\n\n{cooldownText}");
        }
    }

    public void SelectAction()
    {
        BattleManager.Instance.CurrentUnit.ActionHandler.SelectAction(action);
    }
}
