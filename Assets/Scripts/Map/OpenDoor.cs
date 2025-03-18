using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public bool isOpened = false; // 문이 열려있는지 여부
    public float openAngle = 90f; // 문이 열릴 때의 Z축 회전 각도
    public float closeAngle = 0f; // 문이 닫힐 때의 Z축 회전 각도
    public float rotationSpeed = 2f; // 회전 속도

    private float initialX; // X축 고정 값
    private float initialY; // Y축 고정 값
    private float targetZ; // 목표 Z축 회전 값

    private void Start()
    {
        // 초기 X, Y 각도를 저장해서 X, Y는 고정
        initialX = transform.localEulerAngles.x;
        initialY = transform.localEulerAngles.y;
        targetZ = closeAngle; // 시작할 때 닫힌 상태

        transform.localEulerAngles = new Vector3(initialX, initialY, targetZ); // 초기 회전값 설정
    }

    public void ToggleDoor()
    {
        isOpened = !isOpened; // 문 상태 변경
        targetZ = isOpened ? openAngle : closeAngle; // 목표 Z축 회전 변경

        StopAllCoroutines(); // 기존 회전 애니메이션 중지
        StartCoroutine(RotateDoor()); // 문 회전 시작
    }

    private IEnumerator RotateDoor()
    {
        while (Mathf.Abs(transform.localEulerAngles.z - targetZ) > 0.1f)
        {
            float newZ = Mathf.LerpAngle(transform.localEulerAngles.z, targetZ, rotationSpeed * Time.deltaTime);
            transform.localEulerAngles = new Vector3(initialX, initialY, newZ); // X, Y 고정하고 Z축만 회전
            yield return null;
        }
        
        // 최종 위치 보정 (정확한 목표 각도로 설정)
        transform.localEulerAngles = new Vector3(initialX, initialY, targetZ);
    }
}