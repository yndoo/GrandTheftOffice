using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIManager :  Singleton<UIManager>
{
    [Header("UI 패널 참조")]
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private GameClearUI gameClearUI;
    [SerializeField] private TextMeshProUGUI promptText; // 인터랙션 텍스트



    public void ShowInteractionUI(string message)
    {       
        interactionPanel.SetActive(true);
        promptText.text = message;
    }

    public void HideInteractionUI()
    {
        interactionPanel.SetActive(false);
    }

    public void ShowGameClearUI(float clearTime, int score, bool isNewHighScore = false)
    {
        if (gameClearUI != null)
        {
            gameClearUI.ShowClearUI(clearTime, score, isNewHighScore);
        }
    }
}
