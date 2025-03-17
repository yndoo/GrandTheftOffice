using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour, I_Interactable
{
    
    public int stageNumber;
    public bool isClear = false;
    
    private int saveClearedStage; 

    private void Awake()
    {
        saveClearedStage = GameManager.Instance.LastClearedStage;
        if (stageNumber <= saveClearedStage)
        {
            isClear = true;
        }
    }
    

    private void Start()
    {
        // 1. isClear 가 true 면 활성화
        // 2. stageNumber == saveClearedStage + 1 이면 활성화
        // 3. 그 외 비활성화
        if (isClear || stageNumber == saveClearedStage + 1)
        {
            if (isClear)
            {
                ChangeMaterial(Color.green);
            }
            else
            {
                ChangeMaterial(Color.yellow);
            }
            // 자식 오브젝트 활성화
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void OnInteract()
    {
        if (!isClear && stageNumber != saveClearedStage + 1) return;
        
        switch (stageNumber)
        {
            case 1:
                SceneLoaded("MainScene");
                break;
            case 2:
                SceneLoaded("SecondScene");
                break;
            case 3:
                SceneLoaded("ThirdScene");
                break;
            default:
                SceneLoaded("MainScene");
                break;
        }        
    }
    
    private void ChangeMaterial(Color color)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            meshRenderer.material.color = color;
        }
        else
        {
            Debug.LogWarning($"MeshRenderer를 찾을 수 없습니다. ({gameObject.name})");
        }
    }

    private void SceneLoaded(string sceneName)
    {
        SceneManager.Instance.LoadScene(sceneName);
    }

    public string SetPrompt()
    {
        return "aaa";
    }
}

