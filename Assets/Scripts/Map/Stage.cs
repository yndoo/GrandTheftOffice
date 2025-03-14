using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour, IStage
{
    public int stageNumber;
    public bool isClear = false;
    
    public void OnInteract()
    {
        Debug.Log("Stage Start");
        Debug.Log("Stage Number: " + stageNumber);
    }

    public void SetPrompt()
    {
        Debug.Log("Stage End");
    }
}