using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkMatchingZone : MatchingSystem
{
    private Ink currentInk;
    private Rigidbody inkRigidbody;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        // OnTriggerEnter에서 Matching 메서드를 호출하므로 추가 호출은 필요 없음
    }

    public override void Matching(Collider other)
    {
        if (other.TryGetComponent<Ink>(out Ink ink))
        {
            // 잉크 객체를 감지하고 현재 객체로 설정
            currentInk = ink;

            // 위치 고정을 위해 Rigidbody 가져오기
            if (currentInk.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                inkRigidbody = rb;

                // 물리 영향 비활성화
                inkRigidbody.isKinematic = true;

                // 정확한 위치로 이동
                currentInk.transform.position = this.transform.position;

                // X축으로 180도 회전
                Vector3 currentRotation = currentInk.transform.eulerAngles;
                currentRotation.x = 0f;
                currentRotation.y = 0f;
                currentRotation.z = 180f;
                currentInk.transform.eulerAngles = new Vector3(currentRotation.x, currentRotation.y, currentRotation.z);

                // 회전도 고정하려면 다음 코드 추가
                //currentInk.transform.rotation = this.transform.rotation;
            }

            // 즉시 매칭 상태 설정
            IsMatched = true;
        }
    }

    // 현재 매치된 잉크 색상을 반환하는 메서드
    public Color GetInkColor()
    {
        return currentInk != null ? currentInk.GetColor() : Color.clear;
    }

    // 잉크가 영역을 벗어날 때 처리 (필요한 경우)
    protected void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Ink>() == currentInk)
        {
            // 잉크가 나갈 때 물리 영향 다시 활성화 (필요한 경우)
            if (inkRigidbody != null)
            {
                inkRigidbody.isKinematic = false;
            }

            currentInk = null;
            inkRigidbody = null;
            IsMatched = false;
        }
    }
}