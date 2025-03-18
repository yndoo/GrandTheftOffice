using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearTest : MonoBehaviour
{
    public void Start()
    {
        // 타이머 시작
        TimeManager.Instance.StartTimer();
    }

    public void Update()
    {
        //r 키를 눌러 게임클리어 UI 띄우기
        if (Input.GetKeyDown(KeyCode.R))
        {
            UIManager.Instance.ShowGameClearUI();

        }
    }

}
