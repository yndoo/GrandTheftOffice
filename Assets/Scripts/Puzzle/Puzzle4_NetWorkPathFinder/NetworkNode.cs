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