using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BattleOverUI : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] CanvasGroup panel;
    [SerializeField] CanvasGroup quitConfirmationPanel;
    [SerializeField] Button[] buttons;
    [SerializeField] TextMeshProUGUI outcomeText;
    [SerializeField] string playerWonMessage = "Player won!";
    [SerializeField] string playerLostMessage = "Player lost!";
    [SerializeField] string playerFledMessage = "Player fled!";
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] float scaleDuration = 0.5f;

    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        quitConfirmationPanel.gameObject.SetActive(false);
        DisableUI();
    }

    void Start()
    {
        BattleManager.Instance.OnBattleEnded += EnableUI;
    }

    void OnDestroy()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnBattleEnded -= EnableUI;
        }
    }

    void DisableUI()
    {
        canvas.gameObject.SetActive(false);
    }

    void EnableUI(BattleOutcome battleOutcome)
    {
        outcomeText.text = battleOutcome == BattleOutcome.PLAYER_WON
            ? playerWonMessage : battleOutcome == BattleOutcome.PLAYER_LOST
            ? playerLostMessage : playerFledMessage;

        panel.alpha = 0f;
        outcomeText.transform.localScale = Vector3.zero;

        foreach (var button in buttons)
        {
            button.transform.localScale = Vector3.zero;
        }

        canvas.gameObject.SetActive(true);

        var animation = DOTween.Sequence();
        animation.Append(panel.DOFade(1f, fadeDuration))
            .Append(outcomeText.transform.DOScale(Vector3.one, scaleDuration));
        foreach (var button in buttons)
        {
            animation.Append(button.transform.DOScale(Vector3.one, scaleDuration));
        }
    }

    public void Restart()
    {
        SceneLoader.Instance.Restart();
    }

    public void LoadBattleSelection()
    {
        SceneLoader.Instance.LoadBattleSeletion();
    }

    public void LoadMainMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }

    public void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }
}
