using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 노드 타입 열거형
public enum NodeType
{
    Normal,     // 일반 노드
    Amplifier,  // 증폭 노드: 연결된 노드들의 활성화 조건 변경
    Blocker,    // 차단 노드: 특정 노드들의 활성화 방지
    Toggle,     // 토글 노드: 다른 노드 상태 반전
    Timer       // 타이머 노드: 일정 시간 후 비활성화
}

// 기본 네트워크 노드 클래스
[System.Serializable]
public class NetworkNode
{
    public int id;
    public List<int> connections = new List<int>(); // 연결된 노드 ID 목록
    public bool isActive = false;

    // 노드가 활성화된 노드와 직접 연결되어 있는지 확인 (단순 방식)
    public bool IsConnected(HashSet<int> activeNodes)
    {
        // 연결된 노드 중 하나라도 활성화되어 있으면 true
        foreach (int connectedNodeId in connections)
        {
            if (activeNodes.Contains(connectedNodeId))
            {
                return true;
            }
        }
        return false;
    }
}

// 향상된 네트워크 노드 클래스 (새로운 기능 추가)
[System.Serializable]
public class EnhancedNetworkNode : NetworkNode
{
    public NodeType nodeType = NodeType.Normal;             // 노드 타입
    public List<int> affectedNodes = new List<int>();       // 이 노드가 영향을 주는 다른 노드들
    public float timerDuration = 5f;                        // 타이머 노드의 지속 시간
    public int energyCost = 1;                              // 활성화에 필요한 에너지 비용
    public List<int> activationSequence = new List<int>();  // 이 노드 활성화 전에 활성화되어야 하는 노드 순서

    // 노드 설명 (UI에 표시할 수 있음)
    public string description = "";

    // 노드 활성화 조건 확인 (확장된 조건 포함)
    public bool CanActivate(HashSet<int> activeNodes, List<int> activationHistory)
    {
        // 기본 연결 조건
        bool basicCondition = IsConnected(activeNodes);

        // 활성화 순서 조건
        if (activationSequence.Count > 0)
        {
            int lastIndex = -1;
            foreach (int seqNodeId in activationSequence)
            {
                int currentIndex = activationHistory.LastIndexOf(seqNodeId);
                if (currentIndex == -1 || currentIndex <= lastIndex)
                {
                    return false; // 순서가 맞지 않음
                }
                lastIndex = currentIndex;
            }
        }

        // 노드 타입별 추가 조건
        switch (nodeType)
        {
            case NodeType.Amplifier:
                // 증폭 노드의 추가 조건 (예: 최소 2개 이상의 활성 노드와 연결)
                int activeConnections = 0;
                foreach (int connectedId in connections)
                {
                    if (activeNodes.Contains(connectedId))
                    {
                        activeConnections++;
                    }
                }
                return basicCondition && activeConnections >= 2;

            case NodeType.Blocker:
                // 차단 노드의 추가 조건 (예: 차단 대상 노드가 모두 비활성화 상태)
                foreach (int blockedId in affectedNodes)
                {
                    if (activeNodes.Contains(blockedId))
                    {
                        return false; // 차단 대상이 이미 활성화됨
                    }
                }
                return basicCondition;

            default:
                return basicCondition;
        }
    }

    // 노드 효과 활성화 (다른 노드에 영향)
    public void ApplyEffect(List<EnhancedNetworkNode> allNodes, HashSet<int> activeNodes)
    {
        switch (nodeType)
        {
            case NodeType.Toggle:
                // 토글 노드: 지정된 노드 상태 반전
                foreach (int targetId in affectedNodes)
                {
                    EnhancedNetworkNode targetNode = allNodes.Find(n => n.id == targetId);
                    if (targetNode != null && targetNode.nodeType != NodeType.Timer)
                    {
                        // 토글 효과 적용 (실제 토글은 NetworkPuzzle에서 처리)
                        Debug.Log($"토글 효과: 노드 {targetId} 상태 반전 예정");
                    }
                }
                break;

            case NodeType.Amplifier:
                // 증폭 노드: 지정된 노드의 에너지 비용 감소
                foreach (int targetId in affectedNodes)
                {
                    EnhancedNetworkNode targetNode = allNodes.Find(n => n.id == targetId);
                    if (targetNode != null)
                    {
                        int originalCost = targetNode.energyCost;
                        targetNode.energyCost = Mathf.Max(1, targetNode.energyCost - 1);
                        Debug.Log($"증폭 효과: 노드 {targetId} 에너지 비용 {originalCost}→{targetNode.energyCost}");
                    }
                }
                break;
        }
    }

    // 차단 상태인지 확인
    public bool IsBlocked(List<EnhancedNetworkNode> allNodes, HashSet<int> activeNodes)
    {
        foreach (var node in allNodes)
        {
            if (node.isActive && node.nodeType == NodeType.Blocker &&
                node.affectedNodes.Contains(this.id))
            {
                return true;
            }
        }
        return false;
    }
}