using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public AudioSource audioSource;
    public AudioClip putClip;
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
    /// 
    private void Start()
    {
        Time.timeScale = 1.0f;
        audioSource = GetComponent<AudioSource>();
    }
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
