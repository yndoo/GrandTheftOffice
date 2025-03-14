using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMatchedPrinter : Puzzle, IPuzzleCheckable
{
    [SerializeField] private List<MatchingSystem> inkZone;
    [SerializeField] private Color targetColor;
    [SerializeField] private SpriteRenderer colorDisplay;

    private Color combinedColor;

    public void Update()
    {
        // 매 프레임마다 색상 확인 (잉크가 올려질 때마다 바로 반영)
        FindInk();
        // 스프라이트 색상 업데이트
        UpdateColorDisplay();
        if (Input.GetKeyDown(KeyCode.Space))// 추후 상황에 맞게 수정 필요
        {
            CheckAndPrint();
        }
    }

    private void CheckAndPrint()
    {
        if (IsCorrect())
        {
            Debug.Log($"IsCorrect: {combinedColor}");
        }
    }

    // 특정 위치에 있는 InkMatchingZone을 감지하고 색상 조합
    public void FindInk()
    {
        combinedColor = Color.black; // 초기화

        foreach (MatchingSystem zone in inkZone)
        {
            if (zone.IsMatched && zone is InkMatchingZone inkMatchingZone)
            {
                combinedColor += inkMatchingZone.GetInkColor();
            }
        }

        // 색상의 범위를 0~1로 정규화
        combinedColor.r = Mathf.Clamp01(combinedColor.r);
        combinedColor.g = Mathf.Clamp01(combinedColor.g);
        combinedColor.b = Mathf.Clamp01(combinedColor.b);
    }

    // 스프라이트 색상 업데이트
    private void UpdateColorDisplay()
    {
        if (colorDisplay != null)
        {
            colorDisplay.color = combinedColor;
        }
    }

    // 정답 확인 및 종이 출력
    public bool IsCorrect()
    {
        // FindInk()는 매 프레임 호출되므로 여기서 다시 호출할 필요 없음

        float tolerance = 0.05f; // 허용 오차
        bool isCorrect = Mathf.Abs(combinedColor.r - targetColor.r) < tolerance &&
                         Mathf.Abs(combinedColor.g - targetColor.g) < tolerance &&
                         Mathf.Abs(combinedColor.b - targetColor.b) < tolerance;

        if (isCorrect)
        {
            GetReward(); // 정답이면 종이 생성
        }

        return isCorrect;
    }
}