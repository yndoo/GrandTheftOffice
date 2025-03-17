using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NetworkPuzzleHintSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NetworkPuzzle puzzleController;
    [SerializeField] private UnityEngine.UI.Button hintButton;
    [SerializeField] private UnityEngine.UI.Button nextHintButton;
    [SerializeField] private UnityEngine.UI.Button previousHintButton;
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private UnityEngine.UI.Button closeHintButton;
    [SerializeField] private UnityEngine.UI.Button solutionButton;

    [Header("Settings")]
    [SerializeField] private float hintDisplayTime = 5f; // 힌트가 자동으로 사라지는 시간
    [SerializeField] private bool autoHideHint = true;  // 자동으로 힌트를 숨길지 여부

    private int currentNodeHint = 0;
    private Coroutine hideHintCoroutine;

    private void Start()
    {
        // 버튼 이벤트 연결
        if (hintButton != null)
            hintButton.onClick.AddListener(ShowNodeHint);

        if (nextHintButton != null)
            nextHintButton.onClick.AddListener(ShowNextHint);

        if (previousHintButton != null)
            previousHintButton.onClick.AddListener(ShowPreviousHint);

        if (closeHintButton != null)
            closeHintButton.onClick.AddListener(HideHintPanel);

        if (solutionButton != null)
            solutionButton.onClick.AddListener(ShowFullSolution);

        // 초기 상태 설정
        if (hintPanel != null)
            hintPanel.SetActive(false);
    }

    // 현재 선택된 노드에 대한 힌트 표시
    public void ShowNodeHint()
    {
        if (puzzleController == null || hintPanel == null || hintText == null)
            return;

        // 활성화된 노드 중 가장 높은 ID를 가진 노드 찾기
        int highestActiveNodeId = -1;
        foreach (var node in puzzleController.GetNodes())
        {
            if (node.isActive && node.id > highestActiveNodeId)
            {
                highestActiveNodeId = node.id;
            }
        }

        // 활성화된 노드가 없다면 시작 노드에 대한 힌트 표시
        if (highestActiveNodeId == -1)
            highestActiveNodeId = 0;

        // 다음 노드에 대한 힌트 표시
        currentNodeHint = highestActiveNodeId + 1;
        if (currentNodeHint >= puzzleController.GetNodes().Count)
            currentNodeHint = 0;

        ShowHintForNode(currentNodeHint);
    }

    // 다음 노드 힌트 표시
    public void ShowNextHint()
    {
        currentNodeHint++;
        if (currentNodeHint >= puzzleController.GetNodes().Count)
            currentNodeHint = 0;

        ShowHintForNode(currentNodeHint);
    }

    // 이전 노드 힌트 표시
    public void ShowPreviousHint()
    {
        currentNodeHint--;
        if (currentNodeHint < 0)
            currentNodeHint = puzzleController.GetNodes().Count - 1;

        ShowHintForNode(currentNodeHint);
    }

    // 특정 노드에 대한 힌트 표시
    private void ShowHintForNode(int nodeId)
    {
        string hint = puzzleController.GetNodeHint(nodeId);
        ShowHint($"노드 {nodeId} 힌트: {hint}");
    }

    // 완전한 해결책 힌트 표시
    public void ShowFullSolution()
    {
        string solution = puzzleController.GetSolutionHint();
        ShowHint($"해결 경로: {solution}");
    }

    // 힌트 텍스트 표시 및 패널 활성화
    private void ShowHint(string hint)
    {
        if (hintPanel != null && hintText != null)
        {
            hintPanel.SetActive(true);
            hintText.text = hint;

            // 이전 자동 숨김 코루틴 중지
            if (hideHintCoroutine != null)
                StopCoroutine(hideHintCoroutine);

            // 자동 숨김 활성화된 경우 코루틴 시작
            if (autoHideHint)
                hideHintCoroutine = StartCoroutine(HideHintAfterDelay());
        }
    }

    // 힌트 패널 숨기기
    public void HideHintPanel()
    {
        if (hintPanel != null)
            hintPanel.SetActive(false);
    }

    // 일정 시간 후 힌트 자동 숨김
    private IEnumerator HideHintAfterDelay()
    {
        yield return new WaitForSeconds(hintDisplayTime);
        HideHintPanel();
    }

    private void OnDestroy()
    {
        // 이벤트 리스너 제거
        if (hintButton != null)
            hintButton.onClick.RemoveAllListeners();

        if (nextHintButton != null)
            nextHintButton.onClick.RemoveAllListeners();

        if (previousHintButton != null)
            previousHintButton.onClick.RemoveAllListeners();

        if (closeHintButton != null)
            closeHintButton.onClick.RemoveAllListeners();

        if (solutionButton != null)
            solutionButton.onClick.RemoveAllListeners();
    }
}