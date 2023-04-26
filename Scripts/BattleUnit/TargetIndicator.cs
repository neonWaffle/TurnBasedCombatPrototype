using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] float amount = 0.25f;
    [SerializeField] float duration = 0.25f;
    float yInitial;
    float yTarget;
    new Sequence animation;

    void Awake()
    {
        yInitial = transform.position.y;
        yTarget = yInitial + amount;
    }

    void OnEnable()
    {
        animation = DOTween.Sequence();
        animation.SetLoops(-1, LoopType.Restart)
            .Append(transform.DOMoveY(yTarget, duration))
            .Append(transform.DOMoveY(yInitial, duration));
    }

    void OnDisable()
    {
        animation.Kill();
    }
}
