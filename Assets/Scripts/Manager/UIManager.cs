using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIManager :  Singleton<UIManager>
{
    [Header("UI 패널 참조")]
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private GameClearUI gameClearUI;
    [SerializeField] private TextMeshProUGUI promptText; // 인터랙션 텍스트

    [Header("게임 플레이 UI")]
    [SerializeField] private TextMeshProUGUI currentScoreText; // 현재 점수 텍스트
    [SerializeField] private TextMeshProUGUI timerText; // 타이머 텍스트


    protected override void Awake()
    {
        base.Awake();

        // UI 초기화
        if (currentScoreText != null)
        {
            UpdateScoreUI(0);
        }

        if (timerText != null)
        {
            UpdateTimerUI(0);
        }
    }

    public void ShowInteractionUI(string message)
    {       
        interactionPanel.SetActive(true);
        promptText.text = message;
    }

    public void HideInteractionUI()
    {
        interactionPanel.SetActive(false);
    }

    private void OnEnable()
    {
        // ScoreManager와 TimeManager 이벤트 구독
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged += UpdateScoreUI;
        }

        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeUpdated += UpdateTimerUI;
        }
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateScoreUI;
        }

        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeUpdated -= UpdateTimerUI;
        }
    }

    // 현재 점수 UI 업데이트
    private void UpdateScoreUI(int score)
    {
        if (currentScoreText != null)
        {
            currentScoreText.text = "점수: " + score.ToString();
        }
    }

    // 타이머 UI 업데이트
    private void UpdateTimerUI(float time)
    {
        if (timerText != null)
        {
            // 시:분:초.밀리초 형식으로 표시
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);

            timerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
        }
    }
    // 현재 점수와 타이머를 사용하여 게임 클리어 UI 표시 (오버로딩)
    public void ShowGameClearUI()
    {
        if (gameClearUI != null)
        {
            // 타이머 정지 (이미 다른 곳에서 정지시킬 수도 있음)
            TimeManager.Instance.StopTimer();

            // 현재 시간 가져오기
            float clearTime = TimeManager.Instance.GetClearTime();

            // 간단한 ShowClearUI 버전 호출 (float clearTime 만 전달)
            // GameClearUI는 내부적으로 ScoreManager에서 점수와 최고 점수를 가져옵니다
            gameClearUI.ShowClearUI(clearTime);
        }
    }

}
