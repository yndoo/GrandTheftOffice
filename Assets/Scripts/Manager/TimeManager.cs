using System;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    private float gameStartTime;
    private float gameClearTime;
    private bool isTimerRunning = false;
    private float currentTime = 0f;

    public event Action<float> OnTimeUpdated;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("타임매니저 초기화");
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime = Time.time - gameStartTime;
            OnTimeUpdated?.Invoke(currentTime);
        }
    }

    public void StartTimer()
    {
        gameStartTime = Time.time;
        isTimerRunning = true;
        Debug.Log("타이머 시작됨");
    }

    public void StopTimer()
    {
        if (isTimerRunning)
        {
            isTimerRunning = false;
            gameClearTime = currentTime;
            Debug.Log($"타이머 정지: {gameClearTime:F2}초");
        }
    }

    public float GetClearTime()
    {
        return gameClearTime;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public void ResetTimer()
    {
        isTimerRunning = false;
        currentTime = 0f;
        gameStartTime = 0f;
        gameClearTime = 0f;
        Debug.Log("타이머 리셋됨");
    }
}