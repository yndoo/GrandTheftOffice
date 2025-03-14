using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerScreenUI : MonoBehaviour
{
    [HideInInspector] public SendingMailPuzzle puzzle;
    public UnityEngine.UI.Button OpenMessengerBtn;
    public UnityEngine.UI.Button SendMessageBtn;
    public GameObject Messenger;
    public GameObject SentMessage;
    public GameObject Loading;

    private void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;

        OpenMessengerBtn.onClick.AddListener(OnClickOpenMessenger);
        SendMessageBtn.onClick.AddListener(OnClickSendMessage);
    }

    private void OnEnable()
    {
        StartCoroutine(PCStart());
    }

    IEnumerator PCStart()
    {
        Loading.SetActive(true);

        float curTime = 2f;
        while (curTime > 0)
        {
            curTime -= Time.deltaTime;
            yield return null;
        }

        Loading.SetActive(false);
        OpenMessengerBtn.gameObject.SetActive(true);
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

        SentMessage.SetActive(true);

        puzzle?.GetReward();
        Debug.Log("퍼즐3 완료");
    }
}
