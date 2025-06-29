using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    private float countdownToStarttTimer = 3f;
    private float gamePlayingTimer = 0f;
    private float gamePlayingTimerMax = 40f;
    private bool isPause = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        state = State.WaitingToStart;
    }

    private void Start()
    {
        GameInput.instance.OnPause += GameInput_OnPause;
        GameInput.instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPause(object sender, EventArgs e)
    {
        TogglePauseGame();

    }

    private void Update()
    {
        switch (state) 
        {
            case State.WaitingToStart:
                Time.timeScale = 1f;
                break;
            case State.CountdownToStart:
                countdownToStarttTimer -= Time.deltaTime;
                if (countdownToStarttTimer < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                Time.timeScale = 0f;
                SoudManager.instance.SetVolume(0);
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountdownTStartTimer()
    {
        return countdownToStarttTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public void TogglePauseGame()
    {
        if (isPause)
        {
            Time.timeScale = 1f;
            isPause = false;
            OnGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            isPause = true;
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }

    }
}
