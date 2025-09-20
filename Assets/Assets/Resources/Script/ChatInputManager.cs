using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatInputManager : MonoBehaviour
{
    public ChatManager chatManager;
    public InputField inputField; // (Legacy InputField)
    public Button sendButton;

    void Start()
    {
        sendButton.onClick.AddListener(OnSend);
    }

    void OnSend()
    {
        string userText = inputField.text.Trim();
        Debug.Log("사용자 입력: " + userText); // 입력값 확인용 로그

        if (string.IsNullOrEmpty(userText)) return;

        // Player 메시지 추가
        chatManager.AddChat(null, null, userText);

        // 입력창 초기화
        inputField.text = "";

        // AI 응답 테스트
        StartCoroutine(AIReply(userText));
    }

    IEnumerator AIReply(string userText)
    {
        yield return new WaitForSeconds(1f); // 1초 후 응답
        string reply = $"AI: \"{userText}\" 라고 했구나!";
        chatManager.AddChat("AI Bot", null, reply);
    }
}