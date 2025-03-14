using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, I_Interactable
{
    public bool isInteract = false;
    public bool isClickable = true;
    
    private float interactAngle1 = 90f;
    private float interactAngle2 = -90f;
    
    // Prefab 가져ㅑ오기 
    public GameObject firePrefab;

    // 플레이어 인풋 이벤트 발생 시 실행 함수
    public void OnInteract()
    {
        if (isClickable)
        {
            isClickable = false;
            
            isInteract = !isInteract;
            transform.Rotate(new Vector3(0, 0, isInteract ? interactAngle1 : interactAngle2));
            if (isInteract)
            {
                firePrefab.SetActive(true);
            }
            else
            {
                firePrefab.SetActive(false);
            }   
        }
    }

    
    public string SetPrompt()
    {
        string str = isInteract ? "불 끄기" : "불 켜기";
        return str;
    }
}