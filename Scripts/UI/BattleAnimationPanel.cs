using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class BattleAnimationPanel : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] float scaleDuration = 2f;
    [SerializeField] float fadeDuration = 1f;
    public event Action OnAnimationCompleted;

    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
    }

    public void PlayBattleStartAnimation()
    {
        titleText.transform.localScale = Vector3.zero;
        titleText.transform.DOScale(Vector3.one, scaleDuration)
            .OnComplete(() =>
            {
                OnAnimationCompleted?.Invoke();
                titleText.DOFade(0f, fadeDuration)
                    .OnComplete(() => canvas.gameObject.SetActive(false));
            });
    }
}
