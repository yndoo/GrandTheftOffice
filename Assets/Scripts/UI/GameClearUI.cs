using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameClearUI : UIBase
{
    [SerializeField] private TextMeshProUGUI scoreText; // 점수 텍스트
    [SerializeField] private TextMeshProUGUI timeText; // 시간 텍스트
    [SerializeField] private TextMeshProUGUI highScoreText; // 최고 점수 텍스트
    [SerializeField] private UnityEngine.UI.Button restartButton; // 다시 시작 버튼
    [SerializeField] private UnityEngine.UI.Button MainButton; // 메인 버튼

    protected override void Awake()
    {
        base.Awake();
        // 버튼 리스너 추가
        if (restartButton != null) restartButton.onClick.AddListener(OnRestartButtonClicked);
        if (MainButton != null) MainButton.onClick.AddListener(OnMainButtonClicked);
    }

    private void OnEnable()
    {
        // UI가 활성화될 때 ScoreManager의 이벤트 구독
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged += UpdateScoreText;
        }
    }

    private void OnDisable()
    {
        // UI가 비활성화될 때 이벤트 구독 해제
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateScoreText;
        }
    }

    // 점수 텍스트 업데이트 함수
    private void UpdateScoreText(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = newScore.ToString();
        }
    }

    public void ShowClearUI(float clearTime, int score, bool isNewHighScore)
    {
        Show();

        // 현재 점수 표시
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }

        // 클리어 시간 설정
        if (timeText != null)
        {
            timeText.text = clearTime.ToString("0.00");
        }

        if (highScoreText != null)
        {
            // 새로운 최고 점수 여부에 따라 표시
            highScoreText.gameObject.SetActive(isNewHighScore);
        }
    }

    public void Show()
    {
        if (panelRoot != null)
        {
            // 패널 루트 활성화
            panelRoot.SetActive(true);
        }
    }

    public void Hide()
    {
        if (panelRoot != null)
        {
            // 패널 루트 비활성화
            panelRoot.SetActive(false);
        }
    }

    private void OnRestartButtonClicked()
    {
        // 다시 시작 버튼 클릭 시 실행
        // 여기에 게임 재시작 로직 추가
        Debug.Log("재시작 버튼 클릭됨");
    }

    private void OnMainButtonClicked()
    {
        // 메인 버튼 클릭 시 실행
        // 여기에 메인 메뉴로 이동하는 로직 추가
        Debug.Log("메인 버튼 클릭됨");
    }
}