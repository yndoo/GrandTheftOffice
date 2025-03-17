using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIManager :  Singleton<UIManager>
{
    public GameObject interactionPanel; // 인터랙션 UI 패널
    public TextMeshProUGUI promptText; // 인터랙션 텍스트


    public void ShowInteractionText(string message)
    {
        promptText.text = message;
    }


    public void ShowInteractionUI()
    {       
        interactionPanel.SetActive(true);
    }

    public void HideInteractionUI()
    {
        interactionPanel.SetActive(false);
    }

}
