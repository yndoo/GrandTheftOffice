using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameData CurrentGameData {  get; private set; }
    public int LastClearedStage {  get; set; }
    
    public int CurrentStage {  get; set; }

    private void Awake()
    {
        LoadGame();    
    }

    /// <summary>
    /// 세이브데이터를 로드
    /// </summary>
    public void LoadGame()
    {
        CurrentGameData = DataManager.LoadData<GameData>("/SaveData");
        if(CurrentGameData == null) CurrentGameData = new GameData(); 

        LastClearedStage = CurrentGameData.LastClearedChapter;
    }

    /// <summary>
    /// 현재 데이터를 세이브
    /// </summary>
    public void SaveGame()
    {
        CurrentGameData.LastClearedChapter = LastClearedStage;
        DataManager.SaveData<GameData>(CurrentGameData, "/SaveData");
    }

    // 게임 오버 기능
    public void GameOver()
    {
        Debug.Log("GameOver");
    }
}
