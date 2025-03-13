using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerScreenUI : MonoBehaviour
{
    [HideInInspector] public SendingMailPuzzle puzzle;
    public Button OpenMessengerBtn;
    public Button SendMessageBtn;
    public GameObject Messenger;
    private void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;

        OpenMessengerBtn.onClick.AddListener(OnClickOpenMessenger);
        SendMessageBtn.onClick.AddListener(OnClickSendMessage);
    }

    /// <summary>
    /// 메신저 토글
    /// </summary>
    void OnClickOpenMessenger()
    {
        Messenger.SetActive(!Messenger.activeSelf);
    }

    /// <summary>
    /// 사원들에게 메세지 보내는 액션 및 퍼즐 완료 처리
    /// </summary>
    void OnClickSendMessage()
    {
        if (puzzle?.IsCompleted == true) return; // 퍼즐 완료 후 재전송 불가
        // TODO : 전송 완료 UI 활성화

        puzzle?.GetReward();
    }
}
