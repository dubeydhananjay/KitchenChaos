using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public event EventHandler OnStateChanged;
    public event EventHandler<OnGamePausedEventArgs> OnGamePaused;
    public class OnGamePausedEventArgs : EventArgs {
        public bool isGamePaused;
    }
    public static GameManager Instance { get; private set; }
    private float countDownToStartTimer = 30f;
    [SerializeField] private float gameOverTimer = 1f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 100f;

    private enum State {
        WAITINGTOSTART,
        COUNTDOWNTOSTART,
        GAMEPLAYING,
        GAMEOVER
    }

    private State state;
    private bool isGamePaused;
    public bool IsGamePlaying => state == State.GAMEPLAYING;
    public bool IsCountDownActive => state == State.COUNTDOWNTOSTART;
    public bool IsGameOver => state == State.GAMEOVER;
    public float CountDownTimer => countDownToStartTimer;

    private void Awake() {
        Instance = this;
        //state = State.WAITINGTOSTART;
    }

    private void Start() {
        GameInput.Instance.OnGamePaused += GameInput_OnGamePaused;
        GameInput.Instance.OnInteract += GameInput_OnInteract;
        state = State.COUNTDOWNTOSTART;
        OnStateChanged?.Invoke(this,EventArgs.Empty);
    }

    private void GameInput_OnInteract(object sender, EventArgs e) {
        if(state == State.WAITINGTOSTART) {
            state = State.COUNTDOWNTOSTART;
            OnStateChanged?.Invoke(this,EventArgs.Empty);
        }
    }

    private void GameInput_OnGamePaused(object sender, EventArgs e) {
        PauseGame();
    }

    private void Update() {
        switch(state) {
            case State.WAITINGTOSTART:
                break;
             case State.COUNTDOWNTOSTART:
                countDownToStartTimer -= Time.deltaTime;
                if(countDownToStartTimer <= 0) {
                    state = State.GAMEPLAYING;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this,EventArgs.Empty);
                }
                break;
             case State.GAMEPLAYING:
                gamePlayingTimer -= Time.deltaTime;
                if(gamePlayingTimer <= 0) {
                    state = State.GAMEOVER;
                    OnStateChanged?.Invoke(this,EventArgs.Empty);
                }
                break;
             case State.GAMEOVER:
                gameOverTimer -= Time.deltaTime;
                if(gameOverTimer <= 0) {
                    //state = State.COUNTDOWNTOSTART;
                    OnStateChanged?.Invoke(this,EventArgs.Empty);
                }
                break;    
        }
    }

    private void PauseGame() {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0 : 1;
        OnGamePaused?.Invoke(this, new OnGamePausedEventArgs {
            isGamePaused = isGamePaused
        });
    }

    public float GetGamePlayingTimerNormalized() {
        return 1 - (gamePlayingTimer/gamePlayingTimerMax);
    }
}
