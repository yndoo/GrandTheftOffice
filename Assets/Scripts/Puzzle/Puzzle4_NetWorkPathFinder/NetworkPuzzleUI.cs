using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkPuzzleUI : MonoBehaviour
{
    [SerializeField] private NetworkPuzzle puzzleController;

    [Header("UI Elements")]
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Transform nodesContainer;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button resetButton;

    private Dictionary<int, NodeUI> nodeUIElements = new Dictionary<int, NodeUI>();
    private List<LineRenderer> connectionLines = new List<LineRenderer>();

    private void Start()
    {
        // 리셋 버튼 이벤트 연결
        if (resetButton != null)
            resetButton.onClick.AddListener(ResetPuzzle);

        // 네트워크 퍼즐 컨트롤러가 없다면 찾기
        if (puzzleController == null)
            puzzleController = FindObjectOfType<NetworkPuzzle>();

        if (puzzleController != null)
        {
            CreateUI();
            UpdateUI();
        }
        else
        {
            Debug.LogError("NetworkPuzzleUI: NetworkPuzzle controller not found!");
        }
    }

    private void CreateUI()
    {
        // 기존 UI 요소 제거
        foreach (Transform child in nodesContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var line in connectionLines)
        {
            Destroy(line.gameObject);
        }

        nodeUIElements.Clear();
        connectionLines.Clear();

        // 노드 UI 생성
        foreach (var node in puzzleController.GetNodes())
        {
            GameObject nodeObj = Instantiate(nodePrefab, nodesContainer);
            NodeUI nodeUI = nodeObj.GetComponent<NodeUI>();

            if (nodeUI != null)
            {
                nodeUI.Setup(node.id, node.isActive);
                nodeUI.OnNodeClicked += HandleNodeClick;

                // 시작 노드와 목표 노드 표시
                if (puzzleController.IsStartNode(node.id))
                {
                    nodeUI.SetNodeType(NodeType.Start);
                }
                else if (puzzleController.IsTargetNode(node.id))
                {
                    nodeUI.SetNodeType(NodeType.Target);
                }
                else
                {
                    nodeUI.SetNodeType(NodeType.Normal);
                }

                // 위치 설정 (간단한 원형 배치)
                int totalNodes = puzzleController.GetNodes().Count;
                float angle = (360f / totalNodes) * node.id;
                float radius = 150f;
                Vector2 position = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                    Mathf.Sin(angle * Mathf.Deg2Rad) * radius
                );
                nodeObj.GetComponent<RectTransform>().anchoredPosition = position;

                nodeUIElements.Add(node.id, nodeUI);
            }
        }

        // 연결선 생성
        foreach (var node in puzzleController.GetNodes())
        {
            if (!nodeUIElements.ContainsKey(node.id)) continue;

            foreach (int connectedId in node.connections)
            {
                if (!nodeUIElements.ContainsKey(connectedId) || connectedId < node.id) continue;

                // 연결선 생성
                GameObject lineObj = new GameObject($"Line_{node.id}_{connectedId}");
                lineObj.transform.SetParent(nodesContainer);

                LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
                lineRenderer.positionCount = 2;
                lineRenderer.startWidth = 3f;
                lineRenderer.endWidth = 3f;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = Color.gray;
                lineRenderer.endColor = Color.gray;

                RectTransform startRect = nodeUIElements[node.id].GetComponent<RectTransform>();
                RectTransform endRect = nodeUIElements[connectedId].GetComponent<RectTransform>();

                // 월드 좌표로 변환
                Vector3 startPos = startRect.TransformPoint(Vector3.zero);
                Vector3 endPos = endRect.TransformPoint(Vector3.zero);

                lineRenderer.SetPosition(0, startPos);
                lineRenderer.SetPosition(1, endPos);

                connectionLines.Add(lineRenderer);
            }
        }
    }

    private void HandleNodeClick(int nodeId)
    {
        // 노드 토글
        puzzleController.ToggleNode(nodeId);

        // UI 업데이트
        UpdateUI();

        // 퍼즐 상태 확인
        if (puzzleController.IsCorrect())
        {
            statusText.text = "Puzzle Sucess!";
            statusText.color = Color.green;
            puzzleController.GetReward();
        }
        else
        {
            statusText.text = "PlayIng...";
            statusText.color = Color.white;
        }
    }

    private void UpdateUI()
    {
        // 모든 노드 UI 업데이트
        foreach (var node in puzzleController.GetNodes())
        {
            if (nodeUIElements.TryGetValue(node.id, out NodeUI nodeUI))
            {
                nodeUI.UpdateState(node.isActive);
            }
        }

        // 연결선 업데이트 (활성화된 연결은 파란색, 비활성화는 회색)
        foreach (var node in puzzleController.GetNodes())
        {
            foreach (int connectedId in node.connections)
            {
                // 노드 중 하나라도 활성화되어 있으면 연결선 활성화
                bool isConnectionActive = node.isActive ||
                                        puzzleController.IsNodeActive(connectedId);

                // 라인 렌더러 찾기
                LineRenderer line = connectionLines.Find(l =>
                    l.gameObject.name == $"Line_{node.id}_{connectedId}" ||
                    l.gameObject.name == $"Line_{connectedId}_{node.id}");

                if (line != null)
                {
                    line.startColor = isConnectionActive ? Color.blue : Color.gray;
                    line.endColor = isConnectionActive ? Color.blue : Color.gray;
                }
            }
        }
    }

    private void ResetPuzzle()
    {
        puzzleController.ResetPuzzle();
        UpdateUI();
        statusText.text = "퍼즐 리셋됨";
        statusText.color = Color.white;
    }
}

