using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    
    // 게임 오버 기능
    public void GameOver()
    {
        Debug.Log("GameOver");
    }
}
