using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearTest : MonoBehaviour
{
    public void Update()
    {
        float clearTime = TimeManager.Instance.GetClearTime();
        int score = ScoreManager.Instance.GetCurrentScore();
        //r 키를 눌러 게임클리어 UI 띄우기
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 게임클리어 UI 띄우기
            UIManager.Instance.ShowGameClearUI(clearTime, score, true);
        }
    }
}
