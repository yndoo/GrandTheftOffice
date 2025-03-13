using UnityEngine;

public class NetworkLine3D : MonoBehaviour
{
    public Transform startNode;
    public Transform endNode;
    private LineRenderer lineRenderer;

    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // 라인 설정
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;

        // 기본 비활성화 상태로 설정
        SetActive(false);
    }

    private void Update()
    {
        if (startNode == null || endNode == null) return;

        // 노드 위치에 맞춰 라인 위치 업데이트
        lineRenderer.SetPosition(0, startNode.position);
        lineRenderer.SetPosition(1, endNode.position);
    }

    public void SetActive(bool isActive)
    {
        lineRenderer.material = isActive ? activeMaterial : inactiveMaterial;
    }
}