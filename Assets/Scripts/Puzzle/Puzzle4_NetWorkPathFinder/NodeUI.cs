using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// 확장된 노드 타입 열거형
public enum EnhancedNodeType
{
    Normal,     // 일반 노드
    Start,      // 시작 노드
    Target,     // 목표 노드
    Amplifier,  // 증폭 노드
    Blocker,    // 차단 노드
    Toggle,     // 토글 노드
    Timer       // 타이머 노드
}

public class EnhancedNodeUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image timerFillImage;

    public System.Action<int> OnNodeClicked;

    private int nodeId;
    private EnhancedNodeType nodeType = EnhancedNodeType.Normal;
    private int energyCost = 1;
    private float timerDuration = 0f;
    private float remainingTime = 0f;
    private bool isTimerActive = false;

    // 노드 타입별 색상
    private Color normalActiveColor = new Color(0.2f, 0.7f, 1f);
    private Color normalInactiveColor = new Color(0.5f, 0.5f, 0.5f);
    private Color startNodeColor = new Color(0f, 0.8f, 0.2f);
    private Color targetNodeColor = new Color(1f, 0.5f, 0f);
    private Color amplifierColor = new Color(0.7f, 0.2f, 1f);   // 보라색
    private Color blockerColor = new Color(1f, 0.2f, 0.2f);     // 빨간색
    private Color toggleColor = new Color(1f, 0.8f, 0.2f);      // 노란색
    private Color timerColor = new Color(0.2f, 0.8f, 0.8f);     // 청록색
    private Color disabledColor = new Color(0.3f, 0.3f, 0.3f);  // 비활성화 색상

    // 비용 부족 표시 색상
    private Color insufficientEnergyColor = new Color(0.5f, 0f, 0f);

    private Coroutine timerCoroutine;

    public void Setup(int id, bool isActive, EnhancedNodeType type = EnhancedNodeType.Normal, int cost = 1)
    {
        nodeId = id;
        nodeType = type;
        energyCost = cost;

        // 텍스트 업데이트
        idText.text = id.ToString();
        UpdateCostText();

        // 버튼 이벤트 연결
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            OnNodeClicked?.Invoke(nodeId);
        });

        // 초기 상태 설정
        UpdateState(isActive);

        // 타이머 이미지 초기화
        if (timerFillImage != null)
        {
            timerFillImage.fillAmount = 0;
            timerFillImage.gameObject.SetActive(type == EnhancedNodeType.Timer);
        }
    }

    // 노드 상태 업데이트
    public void UpdateState(bool isActive)
    {
        // 타이머 관리
        if (isTimerActive && !isActive)
        {
            StopTimer();
        }

        // 색상 설정
        Color baseColor = GetNodeColor(isActive);
        backgroundImage.color = baseColor;

        // 아이콘 업데이트 (있다면)
        if (iconImage != null)
        {
            UpdateNodeIcon(isActive);
        }
    }

    // 노드 타입에 따른 색상 반환
    private Color GetNodeColor(bool isActive)
    {
        if (!isActive)
        {
            return nodeType == EnhancedNodeType.Target ? new Color(0.7f, 0.3f, 0f) : disabledColor;
        }

        switch (nodeType)
        {
            case EnhancedNodeType.Start:
                return startNodeColor;
            case EnhancedNodeType.Target:
                return targetNodeColor;
            case EnhancedNodeType.Amplifier:
                return amplifierColor;
            case EnhancedNodeType.Blocker:
                return blockerColor;
            case EnhancedNodeType.Toggle:
                return toggleColor;
            case EnhancedNodeType.Timer:
                return timerColor;
            default:
                return normalActiveColor;
        }
    }

    // 노드 타입 설정
    public void SetNodeType(EnhancedNodeType type)
    {
        nodeType = type;

        // 타이머 노드라면 타이머 이미지 활성화
        if (timerFillImage != null)
        {
            timerFillImage.gameObject.SetActive(type == EnhancedNodeType.Timer);
        }
    }

    // 에너지 비용 설정
    public void SetEnergyCost(int cost)
    {
        energyCost = cost;
        UpdateCostText();
    }

    // 비용 텍스트 업데이트
    private void UpdateCostText()
    {
        if (costText != null)
        {
            costText.text = energyCost > 0 ? energyCost.ToString() : "";
        }
    }

    // 에너지 부족 시각 효과
    public void ShowInsufficientEnergy(bool show)
    {
        if (costText != null)
        {
            costText.color = show ? insufficientEnergyColor : Color.white;
        }
    }

    // 타이머 시작
    public void StartTimer(float duration)
    {
        if (nodeType != EnhancedNodeType.Timer) return;

        timerDuration = duration;
        remainingTime = duration;
        isTimerActive = true;

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        timerCoroutine = StartCoroutine(UpdateTimerUI());
    }

    // 타이머 중지
    public void StopTimer()
    {
        isTimerActive = false;
        remainingTime = 0;

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        if (timerFillImage != null)
        {
            timerFillImage.fillAmount = 0;
        }
    }

    // 타이머 UI 업데이트 코루틴
    private IEnumerator UpdateTimerUI()
    {
        while (isTimerActive && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            // 타이머 이미지 채우기 업데이트
            if (timerFillImage != null)
            {
                timerFillImage.fillAmount = remainingTime / timerDuration;
            }

            // 타이머가 끝나면 이벤트 발생 (필요한 경우)
            if (remainingTime <= 0)
            {
                isTimerActive = false;

                // 여기에 타이머 종료 시 실행할 코드 추가
                // 일반적으로 자동 비활성화는 EnhancedNetworkPuzzle에서 처리
            }

            yield return null;
        }
    }

    // 노드 아이콘 업데이트 (필요한 경우)
    private void UpdateNodeIcon(bool isActive)
    {
        // 각 노드 타입에 맞는 아이콘 표시 로직 (필요한 경우)
        iconImage.gameObject.SetActive(nodeType != EnhancedNodeType.Normal);

        // 아이콘 이미지와 색상 설정 로직
        // ...
    }

    // 순서 요구사항 시각 효과 (선택적)
    public void ShowSequenceRequirement(bool show, int sequencePosition = 0)
    {
        // 순서 표시 로직
        // ...
    }

    // 노드가 차단되었음을 표시 (선택적)
    public void ShowBlocked(bool blocked)
    {
        if (blocked)
        {
            // 차단 효과 표시 (예: 잠금 아이콘 표시)
        }
    }
}