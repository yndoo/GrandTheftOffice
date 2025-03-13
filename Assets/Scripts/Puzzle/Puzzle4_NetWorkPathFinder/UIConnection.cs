using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 먼저 UI 연결선 클래스를 만듭니다
public class UIConnectionLine : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image lineImage;

    public int startNodeId;
    public int endNodeId;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        lineImage = GetComponent<Image>();
    }

    public void UpdatePosition(RectTransform start, RectTransform end)
    {
        Vector2 startPos = start.anchoredPosition;
        Vector2 endPos = end.anchoredPosition;

        // 선의 위치는 두 노드의 중간
        rectTransform.anchoredPosition = (startPos + endPos) / 2;

        // 선의 길이 계산
        float distance = Vector2.Distance(startPos, endPos);
        rectTransform.sizeDelta = new Vector2(distance, 3f); // 3픽셀 두께의 선

        // 선의 각도 계산
        Vector2 direction = endPos - startPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetColor(Color color)
    {
        lineImage.color = color;
    }
}