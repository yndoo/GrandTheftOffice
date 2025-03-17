using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 게임 점수를 관리하는 매니저 클래스
/// </summary>
public class ScoreManager : Singleton<ScoreManager>
{
    // 현재 점수
    private int _currentScore = 0;
    public int CurrentScore => _currentScore;

    // 스코어 변경 이벤트 (UI 업데이트에 사용)
    public event Action<int> OnScoreChanged;

    // 스코어 텍스트 UI (인스펙터에서 할당)
    [SerializeField] private TextMeshProUGUI scoreText;

    // Singleton의 Awake를 오버라이드
    protected override void Awake()
    {
        base.Awake(); // 싱글톤 기본 로직 실행

        // 추가 초기화 코드
        UpdateScoreUI();
        Debug.Log("스코어매니저 초기화");
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
}