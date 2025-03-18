using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearTest : MonoBehaviour
{
    public void Start()
    {
        // 타이머 시작
        TimeManager.Instance.StartTimer();
    }

    public void Update()
    {
        float clearTime = TimeManager.Instance.GetCurrentTime();
        int score = ScoreManager.Instance.GetCurrentScore();
        //r 키를 눌러 게임클리어 UI 띄우기
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnGameClear();
            // 게임클리어 UI 띄우기
            UIManager.Instance.ShowGameClearUI(clearTime, score, true);
        }
    }

    public void OnGameClear()
    {
        // 타이머 정지
        TimeManager.Instance.StopTimer();

        // 점수 확인 (최고 점수 체크 등)
        int currentScore = ScoreManager.Instance.GetCurrentScore();
        bool isNewHighScore = true; // 최고 점수인지 여부

        // UI 표시
        UIManager.Instance.ShowGameClearUI(TimeManager.Instance.GetClearTime(), currentScore, isNewHighScore);
    }
}
