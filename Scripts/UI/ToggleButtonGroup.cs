using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButtonGroup : MonoBehaviour
{
    ToggleButton[] toggleButtons;

    void Awake()
    {
        toggleButtons = GetComponentsInChildren<ToggleButton>();
        foreach (var button in toggleButtons)
        {
            button.Init();
        }
    }

    void OnEnable()
    {
        foreach (var button in toggleButtons)
        {
            button.Toggle(false);
        }
    }

    void Start()
    {
        foreach (var button in toggleButtons)
        {
            button.OnToggled += UpdateButtons;
        }
    }

    void OnDestroy()
    {
        foreach (var button in toggleButtons)
        {
            button.OnToggled -= UpdateButtons;
        }
    }

    void UpdateButtons(ToggleButton toggledButton)
    {
        foreach (var button in toggleButtons)
        {
            if (button != toggledButton)
            {
                button.Toggle(false);
            }
        }
    }
}
