using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink : MonoBehaviour
{
    public Color inkColor; // 이 오브젝트의 색상
    private Renderer objectRenderer;
    private Material instanceMaterial;
    private static readonly string ColorPropertyName = "_Colour"; // 쉐이더에서 사용하는 색상 프로퍼티 이름

    void Start()
    {
        // 렌더러 컴포넌트 가져오기
        objectRenderer = GetComponentInChildren<Renderer>();

        if (objectRenderer == null)
        {
            Debug.LogError("Renderer 컴포넌트를 찾을 수 없습니다. 자식 오브젝트에 Renderer가 있는지 확인하세요.");
            return;
        }

        // 메테리얼 인스턴스 생성
        instanceMaterial = new Material(objectRenderer.material);

        // 인스턴스화된 메테리얼 적용
        objectRenderer.material = instanceMaterial;

        // 초기 색상 설정
        UpdateMaterialColor();
    }

    // 색상이 Inspector에서 변경될 때마다 메테리얼에 적용
    void OnValidate()
    {
        if (objectRenderer != null && instanceMaterial != null)
        {
            UpdateMaterialColor();
        }
    }

    // 메테리얼 색상 업데이트
    private void UpdateMaterialColor()
    {
        if (instanceMaterial.HasProperty(ColorPropertyName))
        {
            instanceMaterial.SetColor(ColorPropertyName, inkColor);
        }
        else
        {
            Debug.LogWarning("메테리얼에 " + ColorPropertyName + " 프로퍼티가 없습니다.");
        }
    }

    // 현재 색상 반환
    public Color GetColor()
    {
        return inkColor;
    }

    // 색상 설정 (외부에서 호출 가능)
    public void SetColor(Color newColor)
    {
        inkColor = newColor;
        UpdateMaterialColor();
    }
}