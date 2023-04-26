using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class StatusIndicator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] float duration = 1f;
    [SerializeField] float moveAmount = 1f;

    public void Set(string text, Color messageColour)
    {
        statusText.color = messageColour;
        statusText.text = text;
    }

    public void Animate()
    {
        transform.DOMoveY(transform.position.y + moveAmount, duration);
        Destroy(gameObject, duration);
    }
}
