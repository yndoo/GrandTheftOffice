using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // CCTV 오브젝트들을 담을 리스트
    private List<Enemy> enemies;

    void Start()
    {
        enemies = new List<Enemy>();

        // 씬에서 모든 CCTV 객체를 찾아 리스트에 추가
        CCTV[] cctvObjects = FindObjectsOfType<CCTV>(); // CCTV 스크립트가 붙은 모든 오브젝트 탐색

        foreach (CCTV cctv in cctvObjects)
        {
            enemies.Add(cctv); // CCTV를 적 리스트에 추가
        }
    }

    void Update()
    {
        foreach (var enemy in enemies)
        {
            enemy.Rotate();  // CCTV 회전
            enemy.Detect();  // CCTV 감지 기능
        }
    }
}