using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // 여러 적 객체를 저장하는 리스트
    public List<Enemy> enemies;

    void Start()
    {
        // CCTV 객체들이 자동으로 생성되거나, 에디터에서 직접 할당할 수 있도록 처리
        enemies = new List<Enemy>();

        // 예시로 CCTV 객체 여러 개를 동적으로 생성
        for (int i = 0; i < 5; i++)  // 5개의 CCTV 생성
        {
            GameObject cctvObject = new GameObject("CCTV_" + i); // CCTV 오브젝트 생성
            CCTV cctv = cctvObject.AddComponent<CCTV>(); // CCTV 컴포넌트 추가

            // CCTV의 위치를 랜덤하게 설정하거나, 원하는 위치로 설정할 수 있습니다.
            cctvObject.transform.position = new Vector3(i * 5f, 0f, 0f); // X축 방향으로 간격을 두고 배치

            enemies.Add(cctv); // CCTV 리스트에 추가
        }
    }

    void Update()
    {
        // 모든 적의 동작을 처리
        foreach (var enemy in enemies)
        {
            enemy.Rotate();  // 회전
            enemy.Detect();  // 감지
        }
    }
}