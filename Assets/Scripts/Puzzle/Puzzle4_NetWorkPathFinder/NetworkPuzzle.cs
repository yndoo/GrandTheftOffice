using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class NetworkPuzzle : Puzzle, IPuzzleCheckable
{
    [SerializeField] private List<NetworkNode> nodes = new List<NetworkNode>();
    [SerializeField] private List<int> startNodeIds = new List<int>(); // 초기 활성화 노드
    [SerializeField] private List<int> targetNodeIds = new List<int>(); // 최종 목표 노드

    private HashSet<int> activeNodes = new HashSet<int>();

    private void Start()
    {
        // 초기 노드 활성화
        foreach (int nodeId in startNodeIds)
        {
            activeNodes.Add(nodeId);
            nodes.Find(n => n.id == nodeId).isActive = true;
        }
    }
    
    public void ToggleNode(int nodeId)
    {
        NetworkNode node = nodes.Find(n => n.id == nodeId);
        if (node == null) return;

        // 활성화 시도
        if (!node.isActive)
        {
            // 수정된 부분: CanActivate 메서드 사용
            if (CanActivate(nodeId))
            {
                node.isActive = true;
                activeNodes.Add(nodeId);
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
            Debug.Log($"[ToggleNode] 노드 {nodeId} 비활성화됨");

            // 비활성화 후 네트워크 상태 갱신 (연결이 끊어진 노드들을 자동 비활성화)
            UpdateNetworkState();
        }

        Debug.Log($"[ToggleNode] 노드 {nodeId} 최종 상태: {(node.isActive ? "활성화" : "비활성화")}");
    }

    public bool CanActivate(int nodeId)
    {
        // 기본 연결 조건 확인
        NetworkNode node = nodes.Find(n => n.id == nodeId);
        bool basicCondition = node.IsConnected(activeNodes) || startNodeIds.Contains(nodeId);

        // 추가 패턴 조건
        switch (nodeId)
        {
            case 7: // 예: 노드 7은 노드 1, 3, 5가 모두 활성화되어야 활성화 가능
                return basicCondition && activeNodes.Contains(1) &&
                       activeNodes.Contains(3) && activeNodes.Contains(5);

            case 5: // 예: 노드 5는 노드 1가 비활성화되어 있어야 활성화 가능
                return basicCondition && !activeNodes.Contains(1);

            default:
                return basicCondition;
        }
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
            NetworkNode currentNode = nodes.Find(n => n.id == currentNodeId);

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
                nodes.Find(n => n.id == nodeId).isActive = false;
                activeNodes.Remove(nodeId);
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
    public void SetupPuzzle(List<NetworkNode> puzzleNodes, List<int> startNodes, List<int> targetNodes)
    {
        nodes = puzzleNodes;
        startNodeIds = startNodes;
        targetNodeIds = targetNodes;
    }

    // UI 구현을 위한 추가 메서드들
    public List<NetworkNode> GetNodes()
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

    // 퍼즐 리셋
    public void ResetPuzzle()
    {
        // 모든 노드 비활성화
        foreach (var node in nodes)
        {
            node.isActive = false;
        }

        activeNodes.Clear();

        // 시작 노드 활성화
        foreach (int nodeId in startNodeIds)
        {
            activeNodes.Add(nodeId);
            nodes.Find(n => n.id == nodeId).isActive = true;
        }
    }
   
}