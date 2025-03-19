using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [Header("UI 패널 참조")]
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private GameClearUI gameClearUI;
    [SerializeField] private TextMeshProUGUI promptText; // 인터랙션 텍스트

    [Header("게임 플레이 UI")]
    [SerializeField] private GameObject gameplayUIContainer; // 게임플레이 UI를 감싸는 부모 객체
    [SerializeField] private TextMeshProUGUI currentScoreText; // 현재 점수 텍스트
    [SerializeField] private TextMeshProUGUI timerText; // 타이머 텍스트

    [Header("커서 설정")]
    [SerializeField] private bool hideCursorDuringGameplay = true; // 게임 플레이 중 커서 숨김 여부

    [Header("자동 게임 시작 씬")]
    [SerializeField] private List<string> autoStartGameScenes = new List<string>(); // 자동으로 게임을 시작할 씬 이름 목록

    private bool isGameStarted = false;
    private bool needsUIRefresh = true;

    public bool IsUIActive { get;  set; }

    protected override void Awake()
    {
        base.Awake();

        // UI 참조 초기화
        //RefreshUIReferences();
        IsUIActive = false;
        // 씬 로드 이벤트 리스너 등록
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // 초기 씬에서도 자동 시작 검사
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        CheckAndStartGameForScene(currentSceneName);
    }

    private void Update()
    {
        // UI 갱신이 필요한 경우 실행
        if (needsUIRefresh)
        {
            RefreshUIReferences();
            UpdateUIState();
            //needsUIRefresh = false;
        }
    }

    // UI 참조 새로 찾기
    private void RefreshUIReferences()
    {
        // 참조가 null인 경우에만 찾기
        if (interactionPanel == null)
        {
            interactionPanel = GameObject.Find("InteractionPanel");
        }

        if (gameplayUIContainer == null)
        {
            gameplayUIContainer = GameObject.Find("GameplayUI");
        }

        if (currentScoreText == null)
        {
            GameObject scoreObj = GameObject.Find("ScoreText");
            if (scoreObj != null)
            {
                currentScoreText = scoreObj.GetComponent<TextMeshProUGUI>();
            }
        }

        if (timerText == null)
        {
            GameObject timerObj = GameObject.Find("TimerText");
            if (timerObj != null)
            {
                timerText = timerObj.GetComponent<TextMeshProUGUI>();
            }
        }

        if (gameClearUI == null)
        {
            GameObject clearUIObj = GameObject.Find("GameClearUI");
            if (clearUIObj != null)
            {
                gameClearUI = clearUIObj.GetComponent<GameClearUI>();
            }
        }

        if (promptText == null && interactionPanel != null)
        {
            promptText = interactionPanel.GetComponentInChildren<TextMeshProUGUI>();
        }

        Debug.Log("UI 참조가 갱신되었습니다.");
    }

    // 현재 게임 상태에 따라 UI 갱신
    private void UpdateUIState()
    {
        // UI 초기화
        if (currentScoreText != null && ScoreManager.Instance != null)
        {
            UpdateScoreUI(ScoreManager.Instance.GetCurrentScore());
        }

        if (timerText != null && TimeManager.Instance != null)
        {
            UpdateTimerUI(TimeManager.Instance.GetCurrentTime());
        }

        // 게임플레이 UI 상태 업데이트
        if (gameplayUIContainer != null)
        {
            gameplayUIContainer.SetActive(isGameStarted);
        }

        // 인터랙션 UI 초기 상태
        if (interactionPanel != null)
        {
            //interactionPanel.SetActive(false);
        }

        Debug.Log("UI 상태가 갱신되었습니다. 게임 시작 상태: " + isGameStarted);
    }

    // 씬이 로드될 때마다 호출되는 메서드
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // UI 갱신 필요 표시
        needsUIRefresh = true;

        // 현재 씬 이름 확인
        string currentSceneName = scene.name;

        // 게임 시작 여부 체크
        CheckAndStartGameForScene(currentSceneName);
    }

    // 씬에 따른 게임 시작 여부 검사
    private void CheckAndStartGameForScene(string sceneName)
    {
        // 자동 시작 씬 목록에 현재 씬이 포함되어 있는지 확인
        bool shouldAutoStart = autoStartGameScenes.Contains(sceneName);

        if (shouldAutoStart)
        {
            // 자동 시작 씬이면 게임 시작
            StartGame(true);
            Debug.Log($"씬 '{sceneName}'에서 게임이 자동으로 시작되었습니다.");
        }
        else if (isGameStarted)
        {
            // 이미 게임이 시작된 상태면 UI만 업데이트
            UpdateUIState();
        }
        else
        {
            // 자동 시작 씬이 아니면 게임 종료 상태 유지
            StartGame(false);
        }
    }

    private void OnDestroy()
    {
        // 이벤트 리스너 제거
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ShowInteractionUI(string message)
    {

        if (interactionPanel != null)
        {
            interactionPanel.SetActive(true);
            Debug.Log("인터랙션 UI 표시됨");

            if (promptText != null)
            {
                promptText.text = message;
            }
        }
    }

    public void HideInteractionUI()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
            Debug.Log("인터랙션 UI 숨김");
        }
    }

    private void OnEnable()
    {
        // ScoreManager와 TimeManager 이벤트 구독
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        UnsubscribeFromEvents();
    }

    // 이벤트 구독 메서드
    private void SubscribeToEvents()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged += UpdateScoreUI;
        }

        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeUpdated += UpdateTimerUI;
        }
    }

    // 이벤트 구독 해제 메서드
    private void UnsubscribeFromEvents()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateScoreUI;
        }

        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeUpdated -= UpdateTimerUI;
        }
    }

    // 현재 점수 UI 업데이트
    private void UpdateScoreUI(int score)
    {
        if (currentScoreText != null)
        {
            currentScoreText.text = "점수: " + score.ToString();
        }
    }

    // 타이머 UI 업데이트
    private void UpdateTimerUI(float time)
    {
        if (timerText != null)
        {
            // 시:분:초.밀리초 형식으로 표시
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);
            timerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
            Debug.Log("타이머확인");
        }
    }

    // 커서 가시성 설정
    private void SetCursorVisibility(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // 게임 시작 시 UI 활성화
    public void StartGame(bool startGame)
    {
        isGameStarted = startGame;

        // UI 참조 갱신
        RefreshUIReferences();

        // 게임 시작시 점수 및 타이머 UI 활성화
        if (gameplayUIContainer != null)
        {
            gameplayUIContainer.SetActive(startGame);
        }

        // 게임 시작시 초기화
        if (startGame)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.ResetScore();
                // 이벤트 재구독
                SubscribeToEvents();
            }

            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.ResetTimer();
                TimeManager.Instance.StartTimer();
                // 이벤트 재구독
                SubscribeToEvents();
            }

            // 게임 플레이 중 커서 설정
            if (hideCursorDuringGameplay)
            {
                SetCursorVisibility(false);
            }
        }
        else
        {
            // 게임 종료시 타이머 정지
            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.StopTimer();
            }

            // 게임 종료시 커서 표시
            SetCursorVisibility(true);
        }
    }

    // 씬 이동시 호출되는 메서드
    public void OnSceneTransition()
    {
        // 참조 갱신
        //RefreshUIReferences();

        // 게임플레이 UI 비활성화
        if (gameplayUIContainer != null)
        {
            gameplayUIContainer.SetActive(false);
        }

        // 점수 초기화
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }

        // 타이머 정지 및 초기화
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.StopTimer();
            TimeManager.Instance.ResetTimer();
        }

        // 커서 표시
        SetCursorVisibility(true);

        // 이벤트 구독을 유지하되, 게임 시작 상태는 다음 씬에서 결정
        // isGameStarted = false;
    }

    // 현재 점수와 타이머를 사용하여 게임 클리어 UI 표시
    public void ShowGameClearUI()
    {
        if (gameClearUI == null)
        {
            //RefreshUIReferences();
        }

        if (gameClearUI != null)
        {
            // 타이머 정지 (이미 다른 곳에서 정지시킬 수도 있음)
            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.StopTimer();
            }

            // 현재 시간 가져오기
            float clearTime = TimeManager.Instance != null ? TimeManager.Instance.GetClearTime() : 0f;
            IsUIActive = true;
            // 게임 종료시 커서 표시
            SetCursorVisibility(true);

            // 간단한 ShowClearUI 버전 호출 (float clearTime 만 전달)
            // GameClearUI는 내부적으로 ScoreManager에서 점수와 최고 점수를 가져옵니다
            gameClearUI.ShowClearUI(clearTime);

            // 게임 상태 업데이트
            isGameStarted = false;
        }
        else
        {
            IsUIActive = false;
            Debug.LogError("GameClearUI를 찾을 수 없습니다!");
        }
    }

    // 게임 상태 확인 메서드
    public bool IsGameStarted()
    {
        return isGameStarted;
    }

    // 특정 씬으로 이동하는 메서드
    public void LoadScene(string sceneName)
    {
        // 씬 전환 전 정리 작업
        OnSceneTransition();

        // SceneManager를 통해 씬 로드
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("SceneManager 인스턴스가 없습니다. Unity SceneManager를 사용합니다.");
            // Unity의 내장 SceneManager를 대체로 사용
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }

    // 메인 메뉴 씬으로 이동
    public void LoadMainMenu()
    {
        LoadScene("StageScene");
    }

    // 게임 다시 시작
    public void RestartGame()
    {
        // 현재 씬 다시 로드
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        LoadScene(currentSceneName);
    }

    // 자동 시작 씬 목록에 씬 추가
    public void AddAutoStartScene(string sceneName)
    {
        if (!autoStartGameScenes.Contains(sceneName))
        {
            autoStartGameScenes.Add(sceneName);
        }
    }

    // 자동 시작 씬 목록에서 씬 제거
    public void RemoveAutoStartScene(string sceneName)
    {
        if (autoStartGameScenes.Contains(sceneName))
        {
            autoStartGameScenes.Remove(sceneName);
        }
    }

    // 자동 시작 씬인지 확인
    public bool IsAutoStartScene(string sceneName)
    {
        return autoStartGameScenes.Contains(sceneName);
    }
}