using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIBase : MonoBehaviour
{
    // UI 패널의 루트 게임 오브젝트
    [SerializeField] protected GameObject panelRoot;

    protected virtual void Awake()
    {
        // 기본 상태는 숨김
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }


}

