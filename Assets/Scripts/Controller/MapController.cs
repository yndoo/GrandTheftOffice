using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    // 플레이어 시작위치 잡아주기
    public Transform playerStartPos;
    // 플레이어
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 시작위치 잡아주기
        player.transform.position = playerStartPos.position;
    }
}
