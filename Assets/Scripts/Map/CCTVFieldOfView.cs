using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVFieldOfView : MonoBehaviour
{
    public float detectionRange = 5f;  // 감지 거리
    public float detectionAngle = 45f; // 감지 시야각
    public Material fovMaterial;       // 반투명 머티리얼

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh fovMesh;

    void Start()
    {
        // MeshRenderer 및 MeshFilter 추가
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();

        // 머티리얼 적용 (없으면 자동 생성)
        if (fovMaterial == null)
        {
            fovMaterial = new Material(Shader.Find("Standard"));
            fovMaterial.color = new Color(1, 0, 0, 0.3f); // 빨간색, 반투명
            fovMaterial.SetFloat("_Mode", 3); // Transparent 모드
            fovMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            fovMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            fovMaterial.SetInt("_ZWrite", 0);
            fovMaterial.DisableKeyword("_ALPHATEST_ON");
            fovMaterial.EnableKeyword("_ALPHABLEND_ON");
            fovMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            fovMaterial.renderQueue = 3000;
        }
        
        meshRenderer.material = fovMaterial; // 머티리얼 적용

        fovMesh = new Mesh();
        meshFilter.mesh = fovMesh;
    }

    void Update()
    {
        DrawFOV();
    }

    void DrawFOV()
    {
        int segmentCount = 20; // 원뿔을 구성할 삼각형 개수
        float halfAngle = detectionAngle * 0.5f;
        Vector3 origin = Vector3.zero; // Mesh의 기준점

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        vertices.Add(origin); // 원뿔의 중심점

        for (int i = 0; i <= segmentCount; i++)
        {
            float angle = Mathf.Lerp(-halfAngle, halfAngle, i / (float)segmentCount);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.right;
            vertices.Add(direction * detectionRange);

            if (i > 0)
            {
                triangles.Add(0);
                triangles.Add(i);
                triangles.Add(i + 1);
            }
        }

        fovMesh.Clear();
        fovMesh.vertices = vertices.ToArray();
        fovMesh.triangles = triangles.ToArray();
        fovMesh.RecalculateNormals();
    }
}
