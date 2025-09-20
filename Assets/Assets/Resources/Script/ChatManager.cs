using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [Header("ScrollView Content")]
    public Transform content; // Scroll View → Viewport → Content

    [Header("Prefabs")]
    public GameObject narrationPrefab;
    public GameObject chatPrefab;

    private PlayerData player;
    private string nickname;

    void Start()
    {
        StartCoroutine(InitData());
    }

    private IEnumerator InitData()
    {
        yield return null; // 씬 전환 후 한 프레임 기다림

        if (StartGameManager.Instance != null)
        {
            player = StartGameManager.Instance.GetSelectedPlayer();
            nickname = StartGameManager.Instance.GetUserNickname();
            Debug.Log($"✅ ChatManager 초기화 완료: {player?.id}, {nickname}");
        }
        else
        {
            Debug.LogError("❌ StartGameManager.Instance가 SampleScene에서 발견되지 않음!");
        }
    }



    // ========================
    // 나레이션 추가
    // ========================
    public void AddNarration(string text)
    {
        if (narrationPrefab == null || content == null)
        {
            Debug.LogError("❌ NarrationPrefab 또는 Content가 Inspector에 연결 안 됨!");
            return;
        }

        GameObject narration = Instantiate(narrationPrefab, content, false);

        Transform textObj = narration.transform.Find("Text");
        if (textObj != null)
        {
            textObj.GetComponent<Text>().text = text;
        }
        else
        {
            Debug.LogError("❌ NarrationPrefab 안에 'Text' 오브젝트가 없음!");
        }

        AutoScroll();
    }



    // ========================
    // 채팅 추가 (편의용) → 인수 1개
    // ========================
    public void Add(string text)
    {
        AddChat(null, null, text);
    }

    // ========================
    // 채팅 추가 (전체 지정) → 인수 3개
    // ========================
    public void AddChat(string name, Sprite profile, string text)
    {
        GameObject chat = Instantiate(chatPrefab, content);

        // Profile
        Transform profileObj = chat.transform.Find("Profile");
        if (profileObj != null)
        {
            var img = profileObj.GetComponent<Image>();
            img.sprite = (profile != null) ? profile : (player != null ? player.portrait : null);
        }

        // Name
        Transform nameObj = chat.transform.Find("ChatBox/Name");
        if (nameObj != null)
        {
            var nameText = nameObj.GetComponent<Text>();
            nameText.text = !string.IsNullOrEmpty(name) ? name : player.id;
        }

        // Message
        Transform msgObj = chat.transform.Find("ChatBox/Message");
        if (msgObj != null)
        {
            msgObj.GetComponent<Text>().text = text;
        }

        AutoScroll();
    }

    // ========================
    // 스크롤 맨 아래로 자동 이동
    // ========================
    private void AutoScroll()
    {
        StartCoroutine(ScrollToBottom());
    }

    private IEnumerator ScrollToBottom()
    {
        yield return null; // 1프레임 대기 → 레이아웃 계산 끝난 후 실행
        Canvas.ForceUpdateCanvases();
        ScrollRect scrollRect = content.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 0f;
    }
}
