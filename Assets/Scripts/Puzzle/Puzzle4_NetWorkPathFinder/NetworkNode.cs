using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 네트워크 노드 클래스
[System.Serializable]
public class NetworkNode
{
    public int id;
    public List<int> connections = new List<int>(); // 연결된 노드 ID 목록
    public bool isActive = false;

    // 노드가 활성화되었는지 확인
    // 노드가 연결되어 있는지 확인 (BFS 사용)
    public bool IsConnected(HashSet<int> activeNodes, List<NetworkNode> allNodes)
    {
        if (!activeNodes.Contains(id)) return false; // 현재 노드가 활성화되지 않으면 연결될 수 없음

        Queue<int> queue = new Queue<int>();
        HashSet<int> visited = new HashSet<int>();

        queue.Enqueue(id);
        visited.Add(id);

        Debug.Log($"[IsConnected] 노드 {id}의 연결된 노드 목록: {string.Join(", ", connections)}");

        while (queue.Count > 0)
        {
            int currentNodeId = queue.Dequeue();
            NetworkNode currentNode = allNodes.Find(n => n.id == currentNodeId);

            foreach (int neighborId in currentNode.connections)
            {
                if (!visited.Contains(neighborId) && activeNodes.Contains(neighborId))
                {
                    Debug.Log($"[IsConnected] 노드 {currentNodeId} -> {neighborId} 연결 확인");
                    visited.Add(neighborId);
                    queue.Enqueue(neighborId);
                }
            }
        }

        bool isConnected = visited.Contains(id);
        Debug.Log($"[IsConnected] 노드 {id}의 연결 여부: {isConnected}");

        return isConnected;
    }
}