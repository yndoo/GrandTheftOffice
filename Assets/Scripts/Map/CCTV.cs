using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : Enemy
{
    // 회전 관련
    public float rotationSpeed = 30f;
    private float rotationAngle = 45f;
    private float initialRotation;

    // 감지 관련
    public float detectionDistance = 7f; // 감지 거리
    public float detectionAngle = 45f;   // 시야각

    private CCTVFieldOfView fov;

    void Start()
    {
        fov = GetComponent<CCTVFieldOfView>();
        if (fov == null)
        {
            Debug.LogWarning("CCTVFieldOfView 컴포넌트가 없음! 자동으로 추가합니다.");
            fov = gameObject.AddComponent<CCTVFieldOfView>();
        }

        // FOV 설정 동기화
        fov.detectionRange = detectionDistance;
        fov.detectionAngle = detectionAngle;

        // 현재 배치된 방향을 기준으로 회전 시작
        initialRotation = transform.eulerAngles.y;
    }

    public override void Rotate()
    {
        // 기준 방향에서 좌우 ±rotationAngle 까지 회전
        float targetRotation = initialRotation + Mathf.Sin(Time.time * rotationSpeed * Mathf.Deg2Rad) * rotationAngle;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation, transform.rotation.eulerAngles.z);
    }

    public override void Detect()
    {
        UpdateFOV();
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionDistance);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(playerTag))
            {
                Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.right, directionToTarget); // CCTV가 보는 방향 기준

                if (angleToTarget < detectionAngle * 0.5f)
                {
                    RaycastHit raycastHit;
                    if (Physics.Raycast(transform.position, directionToTarget, out raycastHit, detectionDistance))
                    {
                        if (raycastHit.collider.CompareTag(playerTag))
                        {
                            Debug.Log("Player detected!");
                            OnDetect();
                        }
                    }
                }
            }
        }
    }

    private void UpdateFOV()
    {
        if (fov != null)
        {
            fov.transform.rotation = transform.rotation; // 회전 동기화
        }
    }

    private void OnDetect()
    {
        GameManager.Instance.GameOver();
        Debug.Log("CCTV detected the player!");
    }

    public override void Attack() { }
    public override void Move() { }
}
