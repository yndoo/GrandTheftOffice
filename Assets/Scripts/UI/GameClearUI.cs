using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameClearUI : UIBase
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [SerializeField] private UnityEngine.UI.Button restartButton;
    [SerializeField] private UnityEngine.UI.Button MainButton;


    public void ShowClearUI(float clearTime, int score, bool isNewHighScore)
    {
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        MainButton.onClick.AddListener(OnMainButtonClicked);

        Show();
        scoreText.text = score.ToString();
        timeText.text = clearTime.ToString("0.00");
        if (isNewHighScore)
        {
            highScoreText.gameObject.SetActive(true);
        }
        else
        {
            highScoreText.gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }
    }

    public void Hide()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }

    private void OnRestartButtonClicked()
    {
        //다시하기
    }

    private void OnMainButtonClicked()
    {
        //엘리베이터로 이동
    }
}


