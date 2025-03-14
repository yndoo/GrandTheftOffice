using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TreeEditor.TreeEditorHelper;

public class EnhancedNetworkPuzzleUI : MonoBehaviour
{
    [SerializeField] private EnhancedNetworkPuzzle puzzleController;

    [Header("UI Elements")]
    [SerializeField] private GameObject nodePrefab;  // EnhancedNodeUI 컴포넌트를 가진 프리팹
    [SerializeField] private Transform nodesContainer;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button resetButton;

    [Header("Additional UI")]
    [SerializeField] private TextMeshProUGUI energyText;        // 에너지 표시 텍스트
    [SerializeField] private TextMeshProUGUI timerText;         // 타이머 표시 텍스트
    [SerializeField] private Image timerFillImage;              // 타이머 진행 표시 이미지
    [SerializeField] private GameObject energyPanel;            // 에너지 정보 패널
    [SerializeField] private GameObject timerPanel;             // 타이머 정보 패널

    private Dictionary<int, EnhancedNodeUI> nodeUIElements = new Dictionary<int, EnhancedNodeUI>();
    private List<LineRenderer> connectionLines = new List<LineRenderer>();
    private Coroutine uiUpdateCoroutine;

    private void Start()
    {
        // 리셋 버튼 이벤트 연결
        if (resetButton != null)
            resetButton.onClick.AddListener(ResetPuzzle);

        // 네트워크 퍼즐 컨트롤러가 없다면 찾기
        if (puzzleController == null)
            puzzleController = FindObjectOfType<EnhancedNetworkPuzzle>();

        if (puzzleController != null)
        {
            CreateUI();
            UpdateUI();

            // 주기적 UI 업데이트 시작 (타이머, 에너지 등을 위해)
            if (uiUpdateCoroutine != null)
                StopCoroutine(uiUpdateCoroutine);

            uiUpdateCoroutine = StartCoroutine(UpdateUICoroutine());
        }
        else
        {
            Debug.LogError("EnhancedNetworkPuzzleUI: EnhancedNetworkPuzzle controller not found!");
        }

        // 추가 패널 활성화 (사용 가능한 경우)
        if (energyPanel != null)
            energyPanel.SetActive(true);

        if (timerPanel != null)
            timerPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        if (uiUpdateCoroutine != null)
            StopCoroutine(uiUpdateCoroutine);
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
        var allNodes = puzzleController.GetNodes();
        for (int i = 0; i < allNodes.Count; i++)
        {
            var node = allNodes[i];
            GameObject nodeObj = Instantiate(nodePrefab, nodesContainer);
            EnhancedNodeUI nodeUI = nodeObj.GetComponent<EnhancedNodeUI>();

            if (nodeUI != null)
            {
                // 노드 타입 결정
                EnhancedNodeType nodeType = EnhancedNodeType.Normal;

                if (puzzleController.IsStartNode(node.id))
                {
                    nodeType = EnhancedNodeType.Start;
                }
                else if (puzzleController.IsTargetNode(node.id))
                {
                    nodeType = EnhancedNodeType.Target;
                }

                // 향상된 노드 정보 가져오기 (EnhancedNetworkNode에서 타입과 비용 정보)
                if (node is EnhancedNetworkNode enhancedNode)
                {
                    switch (enhancedNode.nodeType)
                    {
                        case NodeType.Amplifier:
                            nodeType = EnhancedNodeType.Amplifier;
                            break;
                        case NodeType.Blocker:
                            nodeType = EnhancedNodeType.Blocker;
                            break;
                        case NodeType.Toggle:
                            nodeType = EnhancedNodeType.Toggle;
                            break;
                        case NodeType.Timer:
                            nodeType = EnhancedNodeType.Timer;
                            break;
                    }

                    // 노드 설정
                    nodeUI.Setup(node.id, node.isActive, nodeType, enhancedNode.energyCost);
                }
                else
                {
                    // 기본 노드 설정
                    nodeUI.Setup(node.id, node.isActive, nodeType);
                }

                nodeUI.OnNodeClicked += HandleNodeClick;

                // 위치 설정 (간단한 원형 배치 또는 그리드 배치)
                // 원형 배치
                float angle = (360f / allNodes.Count) * i;
                float radius = 180f; // 더 큰 반경 (노드가 많을 경우)
                Vector2 position = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                    Mathf.Sin(angle * Mathf.Deg2Rad) * radius
                );

                // 또는 그리드 배치 (노드가 많은 경우)
                /*
                int columns = Mathf.CeilToInt(Mathf.Sqrt(allNodes.Count));
                int x = i % columns;
                int y = i / columns;
                float cellSize = 100f;
                Vector2 position = new Vector2(
                    (x - (columns - 1) / 2f) * cellSize,
                    (y - (Mathf.CeilToInt(allNodes.Count / (float)columns) - 1) / 2f) * cellSize
                );
                */

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

                // 선의 위치 설정
                UpdateConnectionLine(lineRenderer, node.id, connectedId);
                connectionLines.Add(lineRenderer);
            }
        }
    }

    // 연결선 위치 업데이트
    private void UpdateConnectionLine(LineRenderer line, int startNodeId, int endNodeId)
    {
        if (nodeUIElements.TryGetValue(startNodeId, out EnhancedNodeUI startNode) &&
            nodeUIElements.TryGetValue(endNodeId, out EnhancedNodeUI endNode))
        {
            RectTransform startRect = startNode.GetComponent<RectTransform>();
            RectTransform endRect = endNode.GetComponent<RectTransform>();

            // 월드 좌표로 변환
            Vector3 startPos = startRect.TransformPoint(Vector3.zero);
            Vector3 endPos = endRect.TransformPoint(Vector3.zero);

            line.SetPosition(0, startPos);
            line.SetPosition(1, endPos);
        }
    }

    private void HandleNodeClick(int nodeId)
    {
        // 노드 토글
        puzzleController.ToggleNode(nodeId);

        // UI 업데이트
        UpdateUI();

        // 퍼즐 상태 확인
        CheckPuzzleStatus();
    }

    // 퍼즐 상태 확인
    private void CheckPuzzleStatus()
    {
        if (puzzleController.IsCorrect())
        {
            statusText.text = "퍼즐 성공!";
            statusText.color = Color.green;
            puzzleController.GetReward();

            // 코루틴 정지 (필요한 경우)
            if (uiUpdateCoroutine != null)
            {
                StopCoroutine(uiUpdateCoroutine);
                uiUpdateCoroutine = null;
            }
        }
        else
        {
            statusText.text = "진행 중...";
            statusText.color = Color.white;
        }
    }

    // UI 업데이트 메서드
    private void UpdateUI()
    {
        // 노드 상태 업데이트
        foreach (var node in puzzleController.GetNodes())
        {
            if (nodeUIElements.TryGetValue(node.id, out EnhancedNodeUI nodeUI))
            {
                // 노드 활성화 상태 업데이트
                nodeUI.UpdateState(node.isActive);

                // 추가 상태 업데이트
                if (node is EnhancedNetworkNode enhancedNode)
                {
                    // 타이머 노드인 경우 타이머 상태 업데이트
                    if (enhancedNode.nodeType == NodeType.Timer && node.isActive)
                    {
                        // 타이머 노드 ID가 timerNodes 딕셔너리에 있는지 확인
                        // 실제로는 EnhancedNetworkPuzzle의 public 메서드를 통해 타이머 정보를 얻어야 함
                        float timerDuration = enhancedNode.timerDuration;
                        float remainingTime = timerDuration; // 실제로는 퍼즐 컨트롤러에서 가져와야 함

                        nodeUI.StartTimer(timerDuration);
                    }

                    // 에너지가 부족한 경우 시각적 표시
                    int remainingEnergy = puzzleController.GetRemainingEnergy();
                    nodeUI.ShowInsufficientEnergy(enhancedNode.energyCost > remainingEnergy);

                    // 차단된 노드 표시
                    nodeUI.ShowBlocked(puzzleController.IsBlockedByOtherNode(node.id));
                }
            }
        }

        // 연결선 업데이트
        UpdateConnectionLines();

        // 에너지 정보 업데이트
        if (energyText != null)
        {
            int remainingEnergy = puzzleController.GetRemainingEnergy();
            int totalEnergy = 10; // 총 에너지 (실제로는 퍼즐 컨트롤러에서 가져와야 함)
            energyText.text = $"에너지: {remainingEnergy}/{totalEnergy}";
        }

        // 타이머 정보 업데이트
        if (timerText != null)
        {
            float remainingTime = puzzleController.GetRemainingTime();
            timerText.text = $"시간: {Mathf.CeilToInt(remainingTime)}초";

            if (timerFillImage != null)
            {
                float totalTime = 60f; // 총 제한 시간 (실제로는 퍼즐 컨트롤러에서 가져와야 함)
                timerFillImage.fillAmount = remainingTime / totalTime;
            }
        }
    }

    // 연결선 업데이트
    private void UpdateConnectionLines()
    {
        foreach (var node in puzzleController.GetNodes())
        {
            foreach (int connectedId in node.connections)
            {
                // 노드 중 하나라도 활성화되어 있으면 연결선 활성화
                bool isConnectionActive = node.isActive || puzzleController.IsNodeActive(connectedId);

                // 라인 렌더러 찾기
                LineRenderer line = connectionLines.Find(l =>
                    l.gameObject.name == $"Line_{node.id}_{connectedId}" ||
                    l.gameObject.name == $"Line_{connectedId}_{node.id}");

                if (line != null)
                {
                    // 두 노드가 모두 활성화되면 파란색, 하나만 활성화되면 옅은 파란색, 둘 다 비활성화면 회색
                    Color lineColor;
                    if (node.isActive && puzzleController.IsNodeActive(connectedId))
                    {
                        lineColor = new Color(0f, 0.6f, 1f); // 진한 파란색
                    }
                    else if (isConnectionActive)
                    {
                        lineColor = new Color(0.3f, 0.7f, 0.9f, 0.7f); // 옅은 파란색
                    }
                    else
                    {
                        lineColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // 회색
                    }

                    line.startColor = lineColor;
                    line.endColor = lineColor;

                    // 연결선 위치 업데이트 (UI 요소가 이동한 경우)
                    UpdateConnectionLine(line, node.id, connectedId);
                }
            }
        }
    }

    // 주기적 UI 업데이트를 위한 코루틴
    private IEnumerator UpdateUICoroutine()
    {
        while (true)
        {
            UpdateUI();
            yield return new WaitForSeconds(0.1f); // 100ms마다 업데이트
        }
    }

    // 퍼즐 리셋
    private void ResetPuzzle()
    {
        puzzleController.ResetPuzzle();
        UpdateUI();
        statusText.text = "퍼즐 리셋";
        statusText.color = Color.white;

        // 코루틴 다시 시작 (정지된 경우)
        if (uiUpdateCoroutine == null)
        {
            uiUpdateCoroutine = StartCoroutine(UpdateUICoroutine());
        }
    }
}