using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ScoreManager : Singleton<ScoreManager>
{
    // 현재 점수
    private int _currentScore = 0;
    public int CurrentScore => _currentScore;

    // 최고 점수 (PlayerPrefs에 저장)
    private int _highScore = 0;
    public int HighScore => _highScore;

    // 스코어 변경 이벤트 (UI 업데이트에 사용)
    public event Action<int> OnScoreChanged;

    // 최고 기록 갱신 이벤트
    public event Action<int> OnHighScoreChanged;

    // 스코어 텍스트 UI (인스펙터에서 할당)
    [SerializeField] private TextMeshProUGUI scoreText;

    // 최고 점수 저장 키
    private const string HIGH_SCORE_KEY = "HighScore";

    // Singleton의 Awake를 오버라이드
    protected override void Awake()
    {
        base.Awake(); // 싱글톤 기본 로직 실행

        // 최고 점수 로드
        LoadHighScore();

        // 추가 초기화 코드
        UpdateScoreUI();
        Debug.Log("스코어매니저 초기화");
    }

    // 최고 점수 로드
    private void LoadHighScore()
    {
        _highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        Debug.Log($"최고 점수 로드: {_highScore}");
    }

    // 최고 점수 저장
    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, _highScore);
        PlayerPrefs.Save();
        Debug.Log($"최고 점수 저장: {_highScore}");
    }

    // 현재 점수가 최고 점수인지 확인하고 갱신
    public bool CheckAndUpdateHighScore()
    {
        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            SaveHighScore();
            OnHighScoreChanged?.Invoke(_highScore);
            Debug.Log($"새로운 최고 점수 달성: {_highScore}");
            return true;
        }
        return false;
    }

    // 점수 추가 메서드
    public void AddScore(int amount)
    {
        _currentScore += amount;

        // UI 업데이트
        UpdateScoreUI();

        // 이벤트 발생
        OnScoreChanged?.Invoke(_currentScore);
        Debug.Log($"Score added: {amount}. Total score: {_currentScore}");
    }

    public void SubtractScore(int amount)
    {
        _currentScore -= amount;
        if (_currentScore <= 0)
        {
            _currentScore = 0;
            Debug.Log("점수가 0 이하로 내려갈 수 없습니다.");
        }
        // UI 업데이트
        UpdateScoreUI();
        // 이벤트 발생
        OnScoreChanged?.Invoke(_currentScore);
        Debug.Log($"Score subtracted: {amount}. Total score: {_currentScore}");
    }

    // 점수 초기화 메서드
    public void ResetScore()
    {
        _currentScore = 0;
        UpdateScoreUI();
        OnScoreChanged?.Invoke(_currentScore);
    }

    // UI 업데이트 메서드
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {_currentScore}";
        }
    }

    // 스코어 텍스트 UI 설정 (동적으로 UI 참조 설정 시 사용)
    public void SetScoreText(TextMeshProUGUI text)
    {
        scoreText = text;
        UpdateScoreUI();
    }

    // 현재 점수 가져오기
    public int GetCurrentScore()
    {
        return _currentScore;
    }

    // 최고 점수 가져오기
    public int GetHighScore()
    {
        return _highScore;
    }

    // 최고 점수 초기화 (필요한 경우)
    public void ResetHighScore()
    {
        _highScore = 0;
        SaveHighScore();
        OnHighScoreChanged?.Invoke(_highScore);
        Debug.Log("최고 점수 초기화");
    }
}