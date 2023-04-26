using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldInfoDisplayHandler : MonoBehaviour, IInfoDisplayHandler
{
    [SerializeField] GameObject infoPanel;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;

    void Awake()
    {
        DisableInfo();
    }

    public void DisableInfo()
    {
        infoPanel.SetActive(false);
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
        infoPanel.SetActive(true);
    }
}
