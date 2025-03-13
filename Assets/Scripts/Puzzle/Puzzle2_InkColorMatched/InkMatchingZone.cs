using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkMatchingZone : MatchingSystem
{
    private Ink currentInk;

    // Trigger에 들어오는 즉시 처리
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

            // 위치 조정 (필요한 경우)
            currentInk.transform.position = this.transform.position;

            // 즉시 매칭 상태 설정
            IsMatched = true;
        }
    }

    // 현재 매치된 잉크 색상을 반환하는 메서드
    public Color GetInkColor()
    {
        return currentInk != null ? currentInk.GetColor() : Color.clear;
    }

    // 잉크가 영역을 벗어날 때 처리
    protected void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Ink>() == currentInk)
        {
            currentInk = null;
            IsMatched = false;
        }
    }
}