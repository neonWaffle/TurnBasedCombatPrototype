using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] CanvasGroup quitConfirmationPanel;

    void Awake()
    {
        quitConfirmationPanel.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        SceneLoader.Instance.LoadBattleSeletion();
    }

    public void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }
}
