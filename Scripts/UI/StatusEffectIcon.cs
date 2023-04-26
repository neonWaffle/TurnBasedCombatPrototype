using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(InfoDisplay))]
public class StatusEffectIcon : MonoBehaviour
{
    [SerializeField] CanvasGroup panel;
    InfoDisplay infoDisplay;
    Image statusEffectImage;
    [SerializeField] float fadeDuration = 1f;
    new Tween animation;
    public StatusEffect StatusEffect { get; private set; }

    void Awake()
    {
        infoDisplay = GetComponent<InfoDisplay>();
        statusEffectImage = GetComponent<Image>();
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        gameObject.SetActive(true);

        StatusEffect = statusEffect;
        StatusEffect.OnRemainingTurnsUpdated += UpdateRemainingTurns;

        animation?.Kill();
        panel.alpha = 0f;
        animation = panel.DOFade(1f, fadeDuration);

        statusEffectImage.sprite = statusEffect.StatusEffectSO.Icon;
        infoDisplay.Set($"{statusEffect.StatusEffectSO.Description}\nRemaining turns: {statusEffect.StatusEffectSO.Duration}");
    }

    public void RemoveStatusEffect()
    {
        StatusEffect.OnRemainingTurnsUpdated -= UpdateRemainingTurns;

        animation?.Kill();
        animation = panel.DOFade(0f, fadeDuration).OnComplete(() => gameObject.SetActive(false));

        StatusEffect = null;    
    }

    void UpdateRemainingTurns(int remainingTurns)
    {
        infoDisplay.Set($"{StatusEffect.StatusEffectSO.Description}\nRemaining turns: {remainingTurns}");
    }
}
