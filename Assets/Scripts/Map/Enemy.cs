using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // 감지 범위
    public float detectionRange = 10f;
    // 플레이어 태그
    public string playerTag = "Player";

    // 적의 기본 행동: 감지 및 회전
    public abstract void Detect(); // 감지 기능
    public abstract void Rotate(); // 회전 기능

    // 추가적인 적 행동(예: 공격, 이동 등)
    public virtual void Attack() { }
    public virtual void Move() { }
}