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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckNodeConnections();
        }
    }
    public void ToggleNode(int nodeId)
    {
        Debug.Log("노드 토글: " + nodeId);

        NetworkNode node = nodes.Find(n => n.id == nodeId);
        Debug.Log($"노드 {nodeId} 연결 상태: {node.IsConnected(activeNodes, nodes)}");
        if (node == null) return;

        // 노드 활성화 시
        if (!node.isActive)
        {
            if (node.IsConnected(activeNodes, nodes) || startNodeIds.Contains(nodeId))
            {
                node.isActive = true;
                activeNodes.Add(nodeId);
            }
        }
        // 노드 비활성화 시 (연결이 유지되는지 확인)
        else
        {
            activeNodes.Remove(nodeId);
            if (!node.IsConnected(activeNodes, nodes)) // 연결이 유지되지 않으면 다시 활성화
            {
                activeNodes.Add(nodeId);
            }
            else
            {
                node.isActive = false;
            }
        }

        UpdateNetworkState();
    }


    // 네트워크 상태 업데이트 - 연결이 끊어진 노드 찾아 비활성화
    private void UpdateNetworkState()
    {
        // 활성화된 노드에서 연결된 노드 찾기 (BFS)
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

        // 연결되지 않은 노드 비활성화
        foreach (int nodeId in activeNodes.ToList())
        {
            if (!connectedNodes.Contains(nodeId))
            {
                nodes.Find(n => n.id == nodeId).isActive = false;
                activeNodes.Remove(nodeId);
            }
        }

        Debug.Log($"[UpdateNetworkState] 활성화된 노드 목록: {string.Join(", ", activeNodes)}");
        Debug.Log($"[UpdateNetworkState] 연결된 노드 목록: {string.Join(", ", connectedNodes)}");
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
    void CheckNodeConnections()
    {
        NetworkNode node0 = nodes.Find(n => n.id == 0);
        NetworkNode node1 = nodes.Find(n => n.id == 1);
        NetworkNode node3 = nodes.Find(n => n.id == 3);

        if (node0 == null || node1 == null || node3 == null)
        {
            Debug.LogError("노드를 찾을 수 없습니다.");
            return;
        }

        bool isConnectedTo1 = node0.connections.Contains(1);
        bool isConnectedTo3 = node0.connections.Contains(3);

        Debug.Log($"0번 노드가 1번 노드와 연결되어 있는가? {isConnectedTo1}");
        Debug.Log($"0번 노드가 3번 노드와 연결되어 있는가? {isConnectedTo3}");

        bool isNode0Active = activeNodes.Contains(0);
        bool isNode1Active = activeNodes.Contains(1);
        bool isNode3Active = activeNodes.Contains(3);

        Debug.Log($"0번 노드 활성화 상태: {isNode0Active}");
        Debug.Log($"1번 노드 활성화 상태: {isNode1Active}");
        Debug.Log($"3번 노드 활성화 상태: {isNode3Active}");

        bool isNode1Connected = node1.IsConnected(activeNodes, nodes);
        bool isNode3Connected = node3.IsConnected(activeNodes, nodes);

        Debug.Log($"1번 노드가 활성화된 노드들과 연결되어 있는가? {isNode1Connected}");
        Debug.Log($"3번 노드가 활성화된 노드들과 연결되어 있는가? {isNode3Connected}");
    }
}