using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, I_Interactable
{
    public bool isInteract = false;
    public bool isClickable = true;
    public bool OutTrigger = false;
    
    // Prefab 가져ㅑ오기 
    public GameObject firePrefab;

    // 플레이어 인풋 이벤트 발생 시 실행 함수
    public void OnInteract()
    {
        if (isClickable)
        {
            Debug.Log("Clicked");
            isClickable = false;
            
            isInteract = !isInteract;
            
            firePrefab.SetActive(isInteract);
        }

        if (OutTrigger)
        {
            int saveStage = GameManager.Instance.LastClearedStage;
            int currentStage = GameManager.Instance.CurrentStage;
            
            if (saveStage < currentStage)
            {
                GameManager.Instance.LastClearedStage = currentStage;
            }
            SceneManager.Instance.LoadScene("StageScene");
        }
    }

    
    public string SetPrompt()
    {
        if (OutTrigger)
        {
            return "탈출하시겠습니까?";
        }
        
        if (isClickable)
        {
            return "불을 켜서 게임을 시작하세요.";
        }

        return "물건을 다 챙겨야 탈출할 수 있습니다.";
    }

}