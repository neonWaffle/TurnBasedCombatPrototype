using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoDisplay : MonoBehaviour
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    bool hasInfo = false;
    [SerializeField] float timeTilDisplay = 1.0f;
    RectTransform rectTransform;
    IInfoDisplayHandler infoDisplayHandler;
    Coroutine displayCoroutine;

    void Awake()
    {
        infoDisplayHandler = transform.root.GetComponentInChildren<IInfoDisplayHandler>();
    }

    public Vector3 GetDisplayOffset(bool showOnLeft)
    {
        return showOnLeft
            ? transform.position + new Vector3(-rectTransform.rect.size.x * 0.5f, -rectTransform.rect.size.y * 0.5f, 0)
            : transform.position + new Vector3(rectTransform.rect.size.x * 0.5f, -rectTransform.rect.size.y * 0.5f, 0);
    }

    public void Set(string title, string description)
    {
        Title = title;
        Description = description;
        hasInfo = true;
    }

    public void Set(string description)
    {
        Description = description;
        hasInfo = true;
    }

    public void DisplayInfo()
    {
        if (hasInfo)
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
            displayCoroutine = StartCoroutine(WaitUntilDisplay());
        }
    }

    IEnumerator WaitUntilDisplay()
    {
        yield return new WaitForSecondsRealtime(timeTilDisplay);
        infoDisplayHandler.DisplayInfo(this);
    }

    public void DisableInfo()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        infoDisplayHandler.DisableInfo();
    }

    void OnDisable()
    {
        DisableInfo();
    }
}
