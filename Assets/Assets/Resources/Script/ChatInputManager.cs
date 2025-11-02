using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection; // 리플렉션으로 텍스트 추출
using Google.GenAI;
using System.Threading;

public class ChatInputManager : MonoBehaviour
{
    public ChatManager chatManager;
    public InputField inputField; // (Legacy InputField)
    public Button sendButton;

    [TextArea(3, 8)]
    public string personaPrompt =
        "너는 상냥한 동료야. 반드시 한국어로 답하고, 존댓말을 포함하고 불필요한 이모지는 쓰지 마. " +
        "답변은 친한 직장 동료와 대화하는 것처럼 해. 말이 이어질 수 있도록 사회성 붙여서 대화해";

    // 중복 실행 방지
    private bool _busy = false;

    // 재사용 클라이언트 & 튜닝
    private Client _client;
    private const int UI_UPDATE_INTERVAL_FRAMES = 3; // UI는 3프레임마다 갱신
    private const int STREAM_TIMEOUT_MS = 4000;      // 4초 내 첫 토큰 없으면 폴백

    void Awake()
    {
        var apiKey = "KEY"; // 테스트용 (커밋 금지)
        _client = new Client(apiKey: apiKey);
    }

    void Start()
    {
        // 중복 리스너 방지
        sendButton.onClick.RemoveListener(OnSend);
        sendButton.onClick.AddListener(OnSend);

        // 엔터 전송이 같은 핸들러를 또 호출하지 않도록 정리
        inputField.onEndEdit.RemoveAllListeners();
    }

    void OnSend()
    {
        string userText = inputField.text.Trim();
        if (string.IsNullOrEmpty(userText)) return;
        if (_busy) return;
        _busy = true;

        // 유저 메시지 먼저 렌더
        chatManager.AddChat(null, null, userText);
        inputField.text = "";

        // 다음 프레임부터 AI 시작 (동시 렌더 방지)
        StartCoroutine(AIReplyAfterFrame(userText));
    }

    IEnumerator AIReplyAfterFrame(string userText)
    {
        yield return null;
        yield return new WaitForEndOfFrame();
        yield return StartCoroutine(AIReply(userText));
    }

    IEnumerator AIReply(string userText)
    {
        try
        {
            if (_client == null)
            {
                chatManager.AddChat("현", null, "설정 오류: 클라이언트가 초기화되지 않았습니다.");
                yield break;
            }

            // 캐릭터 프롬프트
            var sys = string.IsNullOrWhiteSpace(personaPrompt)
                ? "너는 한국어로만 간결하게 답한다(한두 문장)."
                : personaPrompt + " 한두 문장으로 간결히.";
            var finalInput = $"[System]\n{sys}\n\n[User]\n{userText}";

            // 업데이트 메서드 존재 확인
            var chatType    = chatManager.GetType();
            var setChatText = chatType.GetMethod("SetChatText", new Type[] { typeof(int), typeof(string) }); // (int, string)
            var setLastText = chatType.GetMethod("SetLastChatText", new Type[] { typeof(string) });          // (string)
            var addChatMi   = chatType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                    .FirstOrDefault(m => m.Name == "AddChat" && m.GetParameters().Length == 3);

            bool canLiveUpdate = (setChatText != null) || (setLastText != null);

            int handle = -1;
            bool createdBubble = false;

            var queue = new ConcurrentQueue<string>();
            var sb = new StringBuilder();
            bool done = false;

            // 스냅샷 길이만 추적 → 빠른 델타
            string lastSnapshot = "";
            int lastSnapshotLen = 0;

            // 타임아웃 제어
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            bool gotFirstToken = false;

            // 스트림 수신
            Task bg = Task.Run(async () =>
            {
                try
                {
                    await foreach (var chunk in _client.Models.GenerateContentStreamAsync(
                                       model: "gemini-2.5-flash",
                                       contents: finalInput
                                   ).WithCancellation(token))
                    {
                        var piece = ExtractAnyText(chunk);
                        if (string.IsNullOrEmpty(piece)) continue;

                        gotFirstToken = true;

                        // 같은 스냅샷이면 무시
                        if (piece.Length == lastSnapshotLen && piece == lastSnapshot) continue;

                        // 델타 계산
                        string delta;
                        if (piece.Length >= lastSnapshotLen && piece.StartsWith(lastSnapshot))
                            delta = piece.Substring(lastSnapshotLen);
                        else
                            delta = piece;

                        if (!string.IsNullOrWhiteSpace(delta))
                            queue.Enqueue(delta);

                        lastSnapshot = piece;
                        lastSnapshotLen = piece.Length;
                    }
                }
                catch (OperationCanceledException) { }
                finally
                {
                    done = true;
                }
            }, token);

            // 첫 토큰 타임아웃 폴백
            float start = Time.realtimeSinceStartup;
            while (!done && !gotFirstToken && (Time.realtimeSinceStartup - start) * 1000 < STREAM_TIMEOUT_MS)
                yield return null;

            if (!gotFirstToken)
            {
                cts.Cancel();

                Task<string> t = Task.Run(async () =>
                {
                    var res = await _client.Models.GenerateContentAsync(
                        model: "gemini-2.5-flash",
                        contents: finalInput
                    );
                    return ExtractAnyText(res) ?? "(빈 응답)";
                });

                yield return new WaitUntil(() => t.IsCompleted);

                var txt = t.Exception != null
                    ? "[에러] " + (t.Exception.InnerException?.Message ?? t.Exception.Message)
                    : t.Result;

                CreateOrUpdateOnce(txt);
                yield break;
            }

            // 스트리밍 UI (간헐 갱신)
            int lastLen = 0;
            int frame = 0;

            while (!done || !queue.IsEmpty)
            {
                while (queue.TryDequeue(out var part))
                    sb.Append(part);

                if (sb.Length > lastLen && (frame++ % UI_UPDATE_INTERVAL_FRAMES == 0))
                {
                    // ✅ 라이브 갱신 가능한 경우에만 스트리밍 도중 말풍선 생성
                    if (canLiveUpdate && !createdBubble && addChatMi != null)
                    {
                        if (addChatMi.ReturnType == typeof(int))
                        {
                            object ret = addChatMi.Invoke(chatManager, new object[] { "현", null, sb.ToString() });
                            handle = ret is int h ? h : -1;
                        }
                        else
                        {
                            addChatMi.Invoke(chatManager, new object[] { "현", null, sb.ToString() });
                            handle = -1;
                        }
                        createdBubble = true;
                    }

                    // 라이브 갱신
                    if (setChatText != null && handle >= 0)
                        setChatText.Invoke(chatManager, new object[] { handle, sb.ToString() });
                    else if (setLastText != null)
                        setLastText.Invoke(chatManager, new object[] { sb.ToString() });

                    lastLen = sb.Length;
                }

                yield return null;
            }

            // 종료 후 최종 고정
            CreateOrUpdateOnce(sb.Length == 0 ? "(빈 응답)" : sb.ToString());

            // ---- 로컬 함수: 말풍선 1회 생성/고정 (중복 생성 방지) ----
            void CreateOrUpdateOnce(string text)
            {
                if (canLiveUpdate)
                {
                    // 스트리밍 중 생성 안 했으면 지금 1회 생성
                    if (!createdBubble && addChatMi != null)
                    {
                        if (addChatMi.ReturnType == typeof(int))
                        {
                            object ret = addChatMi.Invoke(chatManager, new object[] { "현", null, text });
                            handle = ret is int h ? h : -1;
                        }
                        else
                        {
                            addChatMi.Invoke(chatManager, new object[] { "현", null, text });
                            handle = -1;
                        }
                        createdBubble = true;
                    }
                    else
                    {
                        // 이미 생성돼 있으면 새로 추가하지 않고 갱신만
                        if (setChatText != null && handle >= 0) setChatText.Invoke(chatManager, new object[] { handle, text });
                        else if (setLastText != null)          setLastText.Invoke(chatManager, new object[] { text });
                    }
                }
                else
                {
                    // ❗ 라이브 갱신 불가 → 최종에 딱 한 번만 추가
                    if (!createdBubble)
                    {
                        chatManager.AddChat("현", null, text);
                        createdBubble = true;
                    }
                    // 이미 만들어졌더라도(이론상 만들지 않지만) 더 추가하지 않음
                }
            }
        }
        finally
        {
            _busy = false; // 항상 해제하여 다음 입력 허용
        }
    }

    /// <summary>
    /// SDK 차이를 흡수해 텍스트를 최대한 안전하게 추출
    /// 우선순위: Text/OutputText → Candidates[0].Content.Parts[*].Text → InlineData.Data(byte[]) UTF8
    /// </summary>
    private static string ExtractAnyText(object obj)
    {
        if (obj == null) return null;

        var s = GetStringProp(obj, "Text")
             ?? GetStringProp(obj, "OutputText")
             ?? GetStringProp(obj, "Output");
        if (!string.IsNullOrEmpty(s)) return s;

        var candidates = GetIList(obj, "Candidates");
        var cand0 = GetIndex(candidates, 0);
        var content = GetProp(cand0, "Content");
        var parts = GetIList(content, "Parts");

        if (parts != null)
        {
            var sb = new StringBuilder();
            int count = GetCount(parts);
            for (int i = 0; i < count; i++)
            {
                var p = GetIndex(parts, i);
                var txt = GetStringProp(p, "Text");
                if (!string.IsNullOrEmpty(txt)) { sb.Append(txt); continue; }

                var inline = GetProp(p, "InlineData");
                var dataObj = GetProp(inline, "Data");
                if (dataObj is byte[] bytes && bytes.Length > 0)
                {
                    try { sb.Append(Encoding.UTF8.GetString(bytes)); } catch { }
                }
            }
            if (sb.Length > 0) return sb.ToString();
        }

        var fallback = obj.ToString();
        return string.IsNullOrWhiteSpace(fallback) ? null : fallback;
    }

    // ===== 리플렉션 유틸 =====
    private static object GetProp(object o, string name)
    {
        if (o == null) return null;
        var t = o.GetType();
        var pi = t.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
        return pi?.GetValue(o);
    }
    private static string GetStringProp(object o, string name) => GetProp(o, name) as string;
    private static System.Collections.IList GetIList(object o, string name) => GetProp(o, name) as System.Collections.IList;
    private static object GetIndex(object listObj, int index)
    {
        if (listObj is System.Collections.IList l && l.Count > index) return l[index];
        return null;
    }
    private static int GetCount(object listObj) => listObj is System.Collections.IList l ? l.Count : 0;
}
