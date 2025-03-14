using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver,
    LevelComplete
}

public class UIManager :  Singleton<UIManager>
{
    // 현재 게임 상태
    private GameState _currentGameState = GameState.MainMenu;

    // 현재 점수
    private int _currentScore = 0;

    // 현재 레벨
    private int _currentLevel = 1;
}
