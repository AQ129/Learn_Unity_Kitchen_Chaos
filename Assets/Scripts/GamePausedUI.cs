using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button resumeMenuBtn;
    [SerializeField] private Button optionsBtn;

    private void Awake()
    {
        resumeMenuBtn.onClick.AddListener(() => {
            GameManager.instance.TogglePauseGame();
        });

        mainMenuBtn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionsBtn.onClick.AddListener(() =>
        {
            OptionsUI.instance.Show(Show);
            Hide();
        });
    }
    private void Start()
    {
        Hide(); 
        GameManager.instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.instance.OnGameUnPaused += GameManager_OnGameUnPaused;
    }

    private void GameManager_OnGameUnPaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        resumeMenuBtn.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }


}
