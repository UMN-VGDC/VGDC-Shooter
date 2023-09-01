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


    public static Action shootingStart, hasDied, restartGame;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        UpdateGameState(GameState.StartMenu);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.StartMenu:
                restartGame?.Invoke();
                break;
            case GameState.Shooting:
                shootingStart?.Invoke();
                break;
            case GameState.Dead:
                hasDied?.Invoke();
                break;
        }
    }

    public GameState getGameState()
    {
        return State;
    }
}


