using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnhancedNetworkPuzzle : Puzzle, IPuzzleCheckable
{
    [SerializeField] private List<EnhancedNetworkNode> nodes = new List<EnhancedNetworkNode>();
    [SerializeField] private List<int> startNodeIds = new List<int>(); // 초기 활성화 노드
    [SerializeField] private List<int> targetNodeIds = new List<int>(); // 최종 목표 노드
    [SerializeField] private int availableEnergy = 10; // 사용 가능한 총 에너지
    [SerializeField] private float puzzleTimeLimit = 60f; // 퍼즐 제한 시간(초)

    private HashSet<int> activeNodes = new HashSet<int>();
    private List<int> activationHistory = new List<int>(); // 노드 활성화 순서 기록
    private Dictionary<int, float> timerNodes = new Dictionary<int, float>(); // 타이머 노드 추적
    private int usedEnergy = 0; // 사용한 에너지
    private float remainingTime; // 남은 시간
    private bool isTimerActive = false; // 타이머 활성화 상태

    private void Start()
    {
        // 초기 타이머 설정
        remainingTime = puzzleTimeLimit;
        isTimerActive = true;

        // 초기 노드 활성화
        foreach (int nodeId in startNodeIds)
        {
            activeNodes.Add(nodeId);
            nodes.Find(n => n.id == nodeId).isActive = true;
        }

        // 타이머 노드 업데이트 코루틴 시작
        StartCoroutine(UpdateTimerNodes());
    }

    private void Update()
    {
        if (isTimerActive)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                // 시간 초과 시 퍼즐 실패 처리
                PuzzleTimedOut();
            }
        }
    }

    // 시간 초과 처리
    private void PuzzleTimedOut()
    {
        isTimerActive = false;
        Debug.Log("퍼즐 시간 초과!");
        ResetPuzzle();
        // 여기에 시간 초과 UI 표시 등의 로직 추가
    }

    // 타이머 노드 업데이트 코루틴
    private IEnumerator UpdateTimerNodes()
    {
        while (true)
        {
            List<int> nodesToDeactivate = new List<int>();

            foreach (var pair in timerNodes.ToList())
            {
                int nodeId = pair.Key;
                float remainingTime = pair.Value - Time.deltaTime;

                if (remainingTime <= 0)
                {
                    nodesToDeactivate.Add(nodeId);
                }
                else
                {
                    timerNodes[nodeId] = remainingTime;
                }
            }

            // 시간이 다 된 노드 비활성화
            foreach (int nodeId in nodesToDeactivate)
            {
                ToggleNode(nodeId); // 노드 비활성화
                timerNodes.Remove(nodeId);
                Debug.Log($"타이머 종료: 노드 {nodeId} 자동 비활성화");
            }

            yield return null;
        }
    }

    public void ToggleNode(int nodeId)
    {
        EnhancedNetworkNode node = nodes.Find(n => n.id == nodeId);
        if (node == null) return;

        // 활성화 시도
        if (!node.isActive)
        {
            // 에너지 비용 확인
            if (usedEnergy + node.energyCost > availableEnergy)
            {
                Debug.Log($"에너지 부족: 노드 {nodeId} 활성화에 필요한 에너지 {node.energyCost}, 남은 에너지 {availableEnergy - usedEnergy}");
                return;
            }

            // 차단 상태 확인
            if (node.IsBlocked(nodes, activeNodes))
            {
                Debug.Log($"노드 {nodeId}는 다른 차단 노드에 의해 활성화 불가");
                return;
            }

            // 활성화 조건 확인
            if (CanActivate(nodeId))
            {
                // 노드 활성화
                node.isActive = true;
                activeNodes.Add(nodeId);
                activationHistory.Add(nodeId);
                usedEnergy += node.energyCost;

                // 노드 타입별 효과 적용
                node.ApplyEffect(nodes, activeNodes);

                // 타이머 노드인 경우 타이머 시작
                if (node.nodeType == NodeType.Timer)
                {
                    timerNodes[nodeId] = node.timerDuration;
                }

                // 토글 노드인 경우 영향받는 노드들 토글
                if (node.nodeType == NodeType.Toggle)
                {
                    foreach (int toggleId in node.affectedNodes)
                    {
                        // 시작 노드가 아닌 경우에만 토글
                        if (!startNodeIds.Contains(toggleId))
                        {
                            EnhancedNetworkNode targetNode = nodes.Find(n => n.id == toggleId);
                            if (targetNode != null)
                            {
                                // 토글 대상 노드의 상태 변경
                                if (targetNode.isActive)
                                {
                                    targetNode.isActive = false;
                                    activeNodes.Remove(toggleId);
                                    usedEnergy -= targetNode.energyCost; // 에너지 반환
                                }
                                else
                                {
                                    // 토글로 활성화되는 경우 기본 연결 조건은 무시함
                                    targetNode.isActive = true;
                                    activeNodes.Add(toggleId);
                                    usedEnergy += targetNode.energyCost; // 에너지 소모
                                }
                            }
                        }
                    }
                }

                Debug.Log($"노드 {nodeId} 활성화됨");
            }
            else
            {
                Debug.Log($"노드 {nodeId}는 활성화 조건을 충족하지 않음");
            }
        }
        else // 비활성화 시도
        {
            if (startNodeIds.Contains(nodeId))
            {
                Debug.Log($"[ToggleNode] 노드 {nodeId}는 시작 노드이므로 비활성화 불가");
                return; // 시작 노드는 비활성화 불가
            }

            node.isActive = false;
            activeNodes.Remove(nodeId);
            usedEnergy -= node.energyCost; // 에너지 반환

            // 타이머 노드인 경우 타이머 제거
            if (timerNodes.ContainsKey(nodeId))
            {
                timerNodes.Remove(nodeId);
            }

            Debug.Log($"[ToggleNode] 노드 {nodeId} 비활성화됨");

            // 비활성화 후 네트워크 상태 갱신 (연결이 끊어진 노드들을 자동 비활성화)
            UpdateNetworkState();
        }

        Debug.Log($"[ToggleNode] 노드 {nodeId} 최종 상태: {(node.isActive ? "활성화" : "비활성화")}");
    }

    public bool CanActivate(int nodeId)
    {
        EnhancedNetworkNode node = nodes.Find(n => n.id == nodeId);
        if (node == null) return false;

        // 시작 노드는 항상 활성화 가능
        if (startNodeIds.Contains(nodeId)) return true;

        // 확장된 활성화 조건 확인
        return node.CanActivate(activeNodes, activationHistory);
    }


    // 네트워크 상태 업데이트 - 연결이 끊어진 노드 찾아 비활성화
    private void UpdateNetworkState()
    {
        // BFS 탐색을 통해 연결된 노드들을 찾음
        HashSet<int> connectedNodes = new HashSet<int>();
        Queue<int> queue = new Queue<int>();

        foreach (var startNode in startNodeIds)
        {
            queue.Enqueue(startNode);
            connectedNodes.Add(startNode);
        }

        while (queue.Count > 0)
        {
            int currentNodeId = queue.Dequeue();
            EnhancedNetworkNode currentNode = nodes.Find(n => n.id == currentNodeId);

            foreach (int neighborId in currentNode.connections)
            {
                if (!connectedNodes.Contains(neighborId) && activeNodes.Contains(neighborId))
                {
                    connectedNodes.Add(neighborId);
                    queue.Enqueue(neighborId);
                }
            }
        }

        // 연결이 유지되지 않는 노드 자동 비활성화
        foreach (int nodeId in activeNodes.ToList())
        {
            if (!connectedNodes.Contains(nodeId))
            {
                Debug.Log($"[UpdateNetworkState] 노드 {nodeId} 연결이 끊어져 비활성화됨");
                EnhancedNetworkNode node = nodes.Find(n => n.id == nodeId);
                node.isActive = false;
                activeNodes.Remove(nodeId);
                usedEnergy -= node.energyCost; // 에너지 반환

                // 타이머 노드인 경우 타이머 제거
                if (timerNodes.ContainsKey(nodeId))
                {
                    timerNodes.Remove(nodeId);
                }
            }
        }
    }


    public bool IsCorrect()
    {
        // 모든 목표 노드가 활성화되어 있는지 확인
        foreach (int nodeId in targetNodeIds)
        {
            if (!activeNodes.Contains(nodeId))
            {
                return false;
            }
        }
        return true;
    }

    public override void GetReward()
    {
        base.GetReward();
        Debug.Log("네트워크 퍼즐 보상 지급");
        // 여기에 보상 지급 로직 구현
    }

    // 인스펙터에서 노드 데이터를 설정하기 위한 메서드
    public void SetupPuzzle(List<EnhancedNetworkNode> puzzleNodes, List<int> startNodes, List<int> targetNodes)
    {
        nodes = puzzleNodes;
        startNodeIds = startNodes;
        targetNodeIds = targetNodes;
    }

    // UI 구현을 위한 추가 메서드들
    public List<EnhancedNetworkNode> GetNodes()
    {
        return nodes;
    }

    // 노드 활성화 상태 확인
    public bool IsNodeActive(int nodeId)
    {
        return activeNodes.Contains(nodeId);
    }

    // 시작 노드인지 확인
    public bool IsStartNode(int nodeId)
    {
        return startNodeIds.Contains(nodeId);
    }

    // 목표 노드인지 확인
    public bool IsTargetNode(int nodeId)
    {
        return targetNodeIds.Contains(nodeId);
    }

    // 노드가 차단되었는지 확인
    public bool IsBlockedByOtherNode(int nodeId)
    {
        EnhancedNetworkNode node = nodes.Find(n => n.id == nodeId);
        if (node == null) return false;

        return node.IsBlocked(nodes, activeNodes);
    }

    // 남은 에너지 반환
    public int GetRemainingEnergy()
    {
        return availableEnergy - usedEnergy;
    }

    // 남은 시간 반환
    public float GetRemainingTime()
    {
        return remainingTime;
    }

    // 퍼즐 리셋
    public void ResetPuzzle()
    {
        // 모든 노드 비활성화
        foreach (var node in nodes)
        {
            node.isActive = false;
        }

        activeNodes.Clear();
        activationHistory.Clear();
        timerNodes.Clear();
        usedEnergy = 0;
        remainingTime = puzzleTimeLimit;
        isTimerActive = true;

        // 시작 노드 활성화
        foreach (int nodeId in startNodeIds)
        {
            activeNodes.Add(nodeId);
            activationHistory.Add(nodeId);
            nodes.Find(n => n.id == nodeId).isActive = true;
        }
    }
}