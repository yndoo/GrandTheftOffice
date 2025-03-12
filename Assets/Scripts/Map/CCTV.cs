using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : Enemy
{
    // 회전 속도
    public float rotationSpeed = 20f;
    // 회전 범위
    private float rotationAngle = 45f; // 45도 왼쪽에서 오른쪽으로

    // 현재 회전 각도
    private float currentRotation = 0f;

    // 회전 방향: 1 = 시계방향, -1 = 반시계방향
    private int rotationDirection = 1;

    void Start()
    {
        // CCTV 초기화 설정 (필요한 초기 설정이 있을 경우)
    }

    // 감지 기능 구현
    public override void Detect()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange))
        {
            if (hit.collider.CompareTag(playerTag))
            {
                OnDetect();
            }
        }
    }

    // 회전 기능 구현
    public override void Rotate()
    {
        // 시야 회전
        currentRotation += rotationSpeed * rotationDirection * Time.deltaTime;

        // 회전 범위 내에서만 회전하도록 설정
        if (currentRotation >= rotationAngle || currentRotation <= -rotationAngle)
        {
            rotationDirection *= -1;  // 회전 방향 반전
        }

        // CCTV 회전
        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    }

    // 플레이어 감지 시 실행할 동작
    private void OnDetect()
    {
        // GameManager 에서 GameOver 함수 호출
        GameManager.Instance.GameOver();
        Debug.Log("CCTV detected the player!");
        // 추가적인 플레이어 감지 후 처리할 작업을 여기에 추가
    }

    // 추가적인 행동 (예: 공격, 이동 등)을 구현할 수 있음
    public override void Attack()
    {
        // CCTV가 공격하는 방식(만약 있으면)
    }

    public override void Move()
    {
        // CCTV가 이동하는 방식(만약 있으면)
    }
}