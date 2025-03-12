using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintPuzzle : Puzzle, IPuzzleCheckable
{
    public GameObject[] ink; // 배치된 잉크 오브젝트
    public Color targetColor; // 목표 색상
    public GameObject paperPrefab; // 출력할 종이 프리팹
    public Transform printPoint; // 종이가 생성될 위치

    private Color combinedColor; // 조합된 색상

    // 잉크 오브젝트들의 색상을 찾아서 조합
    public void FindInk()
    {
        combinedColor = Color.black; // 초기화 (RGB 조합을 위해 블랙부터 시작)

        foreach (GameObject obj in ink)
        {
            Renderer objRenderer = obj.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                combinedColor += objRenderer.material.color; // RGB 색 조합
            }
        }

        // 색상의 범위를 0~1로 정규화
        combinedColor.r = Mathf.Clamp01(combinedColor.r);
        combinedColor.g = Mathf.Clamp01(combinedColor.g);
        combinedColor.b = Mathf.Clamp01(combinedColor.b);
    }

    // 정답 확인 및 종이 출력
    public bool IsCorrect()
    {
        FindInk(); // 색상 조합 먼저 수행

        // 목표 색상과 비교 (오차 범위 설정 가능)
        float tolerance = 0.05f; // 허용 오차
        bool isCorrect = Mathf.Abs(combinedColor.r - targetColor.r) < tolerance &&
                         Mathf.Abs(combinedColor.g - targetColor.g) < tolerance &&
                         Mathf.Abs(combinedColor.b - targetColor.b) < tolerance;

        if (isCorrect)
        {
            PrintPaper(); // 정답이면 종이 생성
        }

        return isCorrect;
    }

    // 정답이면 종이 출력
    private void PrintPaper()
    {
        if (paperPrefab != null && printPoint != null)
        {
            Instantiate(paperPrefab, printPoint.position, Quaternion.identity);
        }
        Debug.Log("Printed!");
    }
}
