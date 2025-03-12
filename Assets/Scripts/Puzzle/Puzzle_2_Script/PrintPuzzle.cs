using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintPuzzle : Puzzle, IPuzzleCheckable
{
    [SerializeField]private List<Transform> inkPositions; // 감지할 위치 리스트
    [SerializeField] private Color targetColor; // 목표 색상
    [SerializeField] private GameObject paperPrefab; // 출력할 종이 프리팹
    [SerializeField] private Transform printPoint; // 종이가 생성될 위치

    private Color combinedColor; // 조합된 색상

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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

    // 특정 위치에 있는 Ink 스크립트를 감지하고 색상 조합
    public void FindInk()
    {
        combinedColor = Color.black; // 초기화

        foreach (Transform pos in inkPositions)
        {
            Collider[] colliders = Physics.OverlapSphere(pos.position, 0.1f); // 작은 범위 내 충돌 감지

            foreach (Collider col in colliders)
            {
                Ink inkScript = col.GetComponent<Ink>();
                if (inkScript != null)
                {
                    combinedColor += inkScript.GetColor(); // Ink에서 색상을 가져와 조합
                }
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
    }
}
