using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    public event Action OnSceneLoaded;
    bool isLoading = false;

    Canvas canvas;
    [SerializeField] RectTransform leftPanel;
    [SerializeField] RectTransform rightPanel;
    [SerializeField] float transitionDuration = 2f;
    [SerializeField] float transitionPauseDuration = 1f;
    new Sequence animation;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        Load(sceneName);
    }

    public void LoadMainMenu()
    {
        Load("MainMenu");
    }

    public void LoadBattleSeletion()
    {
        Load("BattleSelection");
    }

    public void Restart()
    {
        Load(SceneManager.GetActiveScene().name);
    }

    public bool IsScene(string sceneName)
    {
        return sceneName.Equals(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    void Load(string sceneName)
    {
        if (isLoading)
            return;

        isLoading = true;
        leftPanel.anchoredPosition = new Vector2(-Screen.width * 0.5f, 0);
        rightPanel.anchoredPosition = new Vector2(Screen.width * 0.5f, 0);
        canvas.gameObject.SetActive(true);
        
        if (animation != null && animation.IsPlaying())
        {
            animation.OnComplete(() => StartLoading(sceneName));
        }
        else
        {
            StartLoading(sceneName);
        }
    }

    void StartLoading(string sceneName)
    {
        animation = DOTween.Sequence();
        animation.Append(leftPanel.DOAnchorPos(new Vector2(0, 0), transitionDuration))
            .Join(rightPanel.DOAnchorPos(new Vector2(0, 0), transitionDuration))
            .AppendInterval(transitionPauseDuration)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {
                SceneManager.sceneLoaded += FinishLoading;
                SceneManager.LoadScene(sceneName);
            });
    }

    void FinishLoading(Scene scene, LoadSceneMode mode)
    {
        isLoading = false;
        SceneManager.sceneLoaded -= FinishLoading;

        animation = DOTween.Sequence();
        animation.Append(leftPanel.DOAnchorPos(new Vector2(-Screen.width * 0.5f, 0), transitionDuration))
            .Join(rightPanel.DOAnchorPos(new Vector2(Screen.width * 0.5f, 0), transitionDuration))
            .SetEase(Ease.InOutSine)
            .OnComplete(() => canvas.gameObject.SetActive(false));
    }
}
