using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ToggleButton : MonoBehaviour
{
    public event Action<ToggleButton> OnToggled;
    Image image;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite deselectedSprite;

    public void Init()
    {
        image = GetComponent<Image>();
    }

    public void Toggle(bool isOn)
    {
        if (isOn)
        {
            image.sprite = selectedSprite;
            OnToggled?.Invoke(this);
        }
        else
        {
            image.sprite = deselectedSprite;
        }
    }
}
