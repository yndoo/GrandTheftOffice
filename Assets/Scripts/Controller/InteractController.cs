using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상호작용 가능한 오브젝트의 기본 클래스
public class InteractController : MonoBehaviour
{
    public virtual void OnInteract() 
    {
        // 자식 클래스에서 override 가능하도록 설정
    }
}