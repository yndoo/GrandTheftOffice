using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : Enemy
{
    // 회전 관련
    public float rotationSpeed = 10f;
    private float rotationAngle = 45f;
    private float currentRotation = 0f;
    private int rotationDirection = 1;

    // 감지 관련
    public float detectionRadius = 1f;
    public float detectionDistance = 5f;
    public float detectionAngle = 45f;

    private CCTVFieldOfView fov; // 🔥 CCTVFieldOfView 추가

    void Start()
    {
        fov = GetComponent<CCTVFieldOfView>(); // CCTVFieldOfView 가져오기
        if (fov == null)
        {
            Debug.LogWarning("CCTVFieldOfView 컴포넌트가 없음! 자동으로 추가합니다.");
            fov = gameObject.AddComponent<CCTVFieldOfView>(); // 없으면 추가
        }
    }

    public override void Detect()
    {
        RaycastHit hit;
        Vector3 cctvDirection = transform.right; // CCTV가 감지할 방향

        if (Physics.SphereCast(transform.position, detectionRadius, cctvDirection, out hit, detectionDistance))
        {
            if (hit.collider.CompareTag(playerTag))
            {
                Vector3 directionToTarget = (hit.collider.transform.position - transform.position).normalized;

                if (Vector3.Angle(cctvDirection, directionToTarget) < detectionAngle * 0.5f)
                {
                    OnDetect();
                }
            }
        }
    }

    public override void Rotate()
    {
        currentRotation += rotationSpeed * rotationDirection * Time.deltaTime;

        if (currentRotation >= rotationAngle || currentRotation <= -rotationAngle)
        {
            rotationDirection *= -1;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, currentRotation, transform.rotation.eulerAngles.z);
    }

    private void OnDetect()
    {
        GameManager.Instance.GameOver();
        Debug.Log("CCTV detected the player!");
    }

    public override void Attack() { }
    public override void Move() { }
}
