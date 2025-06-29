using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button playAgainBtn;
    [SerializeField] private Button mainMenuBtn;

    private void Start()
    {
        GameManager.instance.OnStateChanged += GameManager_OnStateChanged;
        playAgainBtn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });
        mainMenuBtn.onClick.AddListener(() => 
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.instance.IsGameOver())
        {
            Show();
            recipesDeliveredText.text = DeliveryManager.instance.GetSuccessfulRecipesAmount().ToString();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        playAgainBtn.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
