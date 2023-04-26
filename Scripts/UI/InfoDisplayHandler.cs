using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoDisplayHandler : MonoBehaviour, IInfoDisplayHandler
{
    [SerializeField] GameObject infoPanel;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] bool infoPanelOnLeft;
    Vector3 infoPanelOffset;

    void Awake()
    {
        var rectTransform = infoPanel.GetComponent<RectTransform>();
        infoPanelOffset = infoPanelOnLeft
            ? new Vector3(-rectTransform.rect.size.x * 0.5f, rectTransform.rect.size.y * 0.5f, 0)
            : new Vector3(rectTransform.rect.size.x * 0.5f, rectTransform.rect.size.y * 0.5f, 0);

        DisableInfo();
    }

    public void DisplayInfo(InfoDisplay infoDisplay)
    {
        if (string.IsNullOrWhiteSpace(infoDisplay.Title))
        {
            titleText.gameObject.SetActive(false);
        }
        else
        {
            titleText.gameObject.SetActive(true);
            titleText.text = infoDisplay.Title;
        }

        descriptionText.text = infoDisplay.Description;
        infoPanel.transform.position = infoDisplay.GetDisplayOffset(infoPanelOnLeft) + infoPanelOffset;
        infoPanel.SetActive(true);
    }

    public void DisableInfo()
    {
        infoPanel.SetActive(false);
    }
}
