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
            ScoreManager.Instance.OnHighScoreChanged += UpdateHighScoreText;
        }
    }

    private void OnDisable()
    {
        // UI가 비활성화될 때 이벤트 구독 해제
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateScoreText;
            ScoreManager.Instance.OnHighScoreChanged -= UpdateHighScoreText;
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

    // 최고 점수 텍스트 업데이트 함수
    private void UpdateHighScoreText(int newHighScore)
    {
        if (highScoreText != null && highScoreText.gameObject.activeSelf)
        {
            highScoreText.text = "최고 점수: " + newHighScore.ToString();
        }
    }

    public void ShowClearUI(float clearTime)
    {
        Show();

        // 현재 점수 가져오기
        int currentScore = ScoreManager.Instance.GetCurrentScore();

        // 최고 점수 확인 및 갱신
        bool isNewHighScore = ScoreManager.Instance.CheckAndUpdateHighScore();

        // 현재 점수 표시
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
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

            if (isNewHighScore)
            {
                highScoreText.text = "최고 점수: " + ScoreManager.Instance.GetHighScore().ToString();
            }
        }
    }

    // 기존 오버로드된 메서드 (이전 버전과의 호환성 유지)
    public void ShowClearUI(float clearTime, int score, bool isNewHighScore)
    {
        // 새 로직을 사용하는 메서드 호출
        ShowClearUI(clearTime);
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

        // 게임 재시작 시 점수 초기화
        ScoreManager.Instance.ResetScore();

        // UI 숨기기
        Hide();
    }

    private void OnMainButtonClicked()
    {
        // 메인 버튼 클릭 시 실행
        // 여기에 메인 메뉴로 이동하는 로직 추가
        Debug.Log("메인 버튼 클릭됨");

        // UI 숨기기
        Hide();
    }
}