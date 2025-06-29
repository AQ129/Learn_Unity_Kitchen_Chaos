using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameStartCountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private Animator animator;
    private int previousCount;
    private int currentCount;
    private const string NUMBER_POP_UP = "NumberPopUp";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void Update()
    {
        currentCount = Mathf.CeilToInt(GameManager.instance.GetCountdownTStartTimer());
        countdownText.text = currentCount.ToString();
        if (previousCount != currentCount)
        {
            previousCount = currentCount;
            animator.SetTrigger(NUMBER_POP_UP);
            SoudManager.instance.PlayCountDownSound();
        }
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
