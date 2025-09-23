using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject stagePanel;

    [Header("Login UI")]
    public Dropdown playerDropdown;  // Player1 / Player2
    public InputField nicknameInput; // 유저 닉네임
    public Button submitButton;

    [Header("Players")]
    public PlayerData[] players; // Inspector에서 Player1, Player2 등록

    public static StartGameManager Instance;

    private PlayerData selectedPlayer;
    private string userNickname; // ✅ 유저 입력 닉네임 따로 저장

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        loginPanel.SetActive(true);
        stagePanel.SetActive(false);
        submitButton.interactable = false;

        playerDropdown.onValueChanged.AddListener(delegate { CheckSubmitAvailable(); });
        nicknameInput.onValueChanged.AddListener(delegate { CheckSubmitAvailable(); });
    }

    private void CheckSubmitAvailable()
    {
        bool dropdownSelected = playerDropdown.value >= 0;
        bool nicknameEntered = !string.IsNullOrEmpty(nicknameInput.text);
        submitButton.interactable = dropdownSelected && nicknameEntered;
    }

    public void OnSubmit()
    {
        int playerIndex = playerDropdown.value;

        if (playerIndex < 0 || playerIndex >= players.Length)
        {
            Debug.LogError($"플레이어 인덱스 범위 초과! Dropdown: {playerDropdown.value}, Players Length: {players.Length}");
            return;
        }

        selectedPlayer = players[playerIndex];
        userNickname = nicknameInput.text; // ✅ 유저 입력 닉네임 따로 저장

        Debug.Log($"선택된 캐릭터: {selectedPlayer.id}, 유저 닉네임: {userNickname}");

        loginPanel.SetActive(false);
        stagePanel.SetActive(true);
    }

    public void OnSelectStage(int stage)
    {
        if (stage == 1)
        {
            SceneManager.LoadScene("Session_1-1");
        }
    }

    // ✅ 다른 씬에서 쓸 수 있도록 Getter 제공
    public PlayerData GetSelectedPlayer() => selectedPlayer;
    public string GetUserNickname() => userNickname;
}