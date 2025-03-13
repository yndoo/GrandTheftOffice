using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// 네트워크 노드 클래스
[System.Serializable]
public class NetworkNode
{
    public int id;
    public List<int> connections = new List<int>(); // 연결된 노드 ID 목록
    public bool isActive = false;

    // 노드가 활성화되었는지 확인
    public bool IsConnected(HashSet<int> activeNodes)
    {
        // 연결된 노드 중 하나라도 활성화되어 있으면 true
        return connections.Any(nodeId => activeNodes.Contains(nodeId));
    }
}

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

    // 노드 활성화/비활성화 토글
    public void ToggleNode(int nodeId)
    {
        NetworkNode node = nodes.Find(n => n.id == nodeId);
        if (node == null) return;

        // 노드가 이미 활성화된 노드와 연결되어 있거나, 시작 노드인 경우에만 토글 가능
        if (node.IsConnected(activeNodes) || startNodeIds.Contains(nodeId))
        {
            node.isActive = !node.isActive;

            if (node.isActive)
            {
                activeNodes.Add(nodeId);
            }
            else
            {
                activeNodes.Remove(nodeId);
            }

            // 네트워크 전체 상태 업데이트 (연결이 끊어진 노드 비활성화)
            UpdateNetworkState();
        }
    }

    // 네트워크 상태 업데이트 - 연결이 끊어진 노드 찾아 비활성화
    private void UpdateNetworkState()
    {
        bool changed;
        do
        {
            changed = false;
            List<int> nodesToDeactivate = new List<int>();

            // 활성화된 노드 중 연결이 끊어진 노드 찾기
            foreach (int nodeId in activeNodes.ToList())
            {
                // 시작 노드는 항상 활성 상태 유지
                if (startNodeIds.Contains(nodeId)) continue;

                NetworkNode node = nodes.Find(n => n.id == nodeId);
                if (!node.IsConnected(activeNodes))
                {
                    nodesToDeactivate.Add(nodeId);
                    changed = true;
                }
            }

            // 연결이 끊어진 노드 비활성화
            foreach (int nodeId in nodesToDeactivate)
            {
                activeNodes.Remove(nodeId);
                nodes.Find(n => n.id == nodeId).isActive = false;
            }
        } while (changed); // 더 이상 변화가 없을 때까지 반복
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