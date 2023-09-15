using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    StartMenu,
    TutorialStage,
    Shooting,
    Paused,
    Dead
}
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameState State;


    public static Action shootingStart, hasDied, restartGame, paused, stateChange, tutorialStage;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        UpdateGameState(GameState.StartMenu);
        DummyTarget.counter = 0;
        Shoot.isShooting = false;
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.StartMenu:
                Cursor.visible = true;
                restartGame?.Invoke();
                break;
            case GameState.TutorialStage:
                Cursor.visible = false;
                tutorialStage?.Invoke();
                break;
            case GameState.Shooting:
                Cursor.visible = false;
                shootingStart?.Invoke();
                break;
            case GameState.Paused:
                Cursor.visible = true;
                paused?.Invoke();
                break;
            case GameState.Dead:
                Cursor.visible = true;
                hasDied?.Invoke();
                break;
        }

        stateChange?.Invoke();
    }

    public GameState getGameState()
    {
        return State;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    private GameState previousState;
    public void PauseUnpause()
    {
        if (getGameState() == GameState.StartMenu || getGameState() == GameState.Dead) return;
        if (getGameState() == GameState.Paused)
        {
            UpdateGameState(previousState);
            return;
        }

        previousState = getGameState();
        UpdateGameState(GameState.Paused);
    }
}


