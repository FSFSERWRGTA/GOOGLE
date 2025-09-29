using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;
    
    public Sprite[] portraitArr;
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();

    }

    // Update is called once per frame
    void GenerateData()
    {
        //talkData.Add(1000, new string[]{"행인1:그거 들었어? 저기에 새로운 귀신이 돌아다닌다는 소문이 돌던데.:0", "행인2:뭐 정말?:1"});
        //NPC 대사
        talkData.Add(1000, new string[] { "성이한:그거 들었어요?:0", "백하연:그거라뇨?:1", "성이한:저기 저 음악 방송할 때 쓰는 세트장 있죠? 저기에 귀신이 나온대요.:0", "백하연: 귀신이요?:1", "성이한:하연 씨도 들어본 적 있다고 하지 않았어요? 퇴근할 때마다 이상한 노랫소리 같은 게 들린다면서요.:0", "백하연:그렇긴 한데요. 전 당연히 야근해서 환청이 들리는 줄 알았죠. 한 새벽 3시쯤 퇴근하는 날에 들은 거라.:1", "성이한:요즘 새로 기획한다는 오디션 프로그램 때문에 그런다고 했나?:0", "백하연:네 맞아요. 출연진 101명 섭외하는 것도 일이라서요. 101마리 달마시안도 아니고....:1", "성이한:아무튼, 하연 씨 말고도 새벽 3~4시쯤에 퇴근하는 사람들이 다 한 번씩 들었다나 봐요. 문을 열면 갑자기 노랫소리도 멈추고, 조명이 꺼진다는데.... 참 이상하죠?:0", "백하연:다들 요즘 많이 힘든가 보네요. 요즘 같은 시대에 귀신이라니....:1", "성이한:진짜일 수도 있죠.:0" });
        talkData.Add(1001, new string[] { "이지아:이번에 배우정 드라마 들어가는 거 보셨어요? 아이돌 역할이던데.:1", "성나연:아이돌 출신이라 그런가. 잘 어울리는 것 같더라고요.:2", "이지아:배우정이 연기를 너무 잘 해서 그런가. 자꾸 아이돌이라는 걸 까먹게 되네요.:1", "성나연:뭐, 그룹보다 본인 브랜드파워가 강해서 그런 거니 별 수 있나요. 그나저나 배우정 노래 하나는 진짜 잘하던데. 표정도 잘 쓰고. 이번 드라마에 쓰는 노래를 좀 잘 뽑았으면 좋겠네요.:2", "이지아:메인보컬 출신이랬던가요? 그 그룹 노래는 나쁘지 않았던 것 같은데 소속사가 영 일을 못 하더라고요.:1", "성나연:애초에 배우 기획사에서 돈 좀 벌어보려고 기획한 걸그룹이니 별 수 있나요. 배우 하려다 강제로 아이돌로 데뷔하고 묻힌 애들만 불쌍한 거죠.:2" });
        talkData.Add(1002, new string[] { "진하연:입장 몇 시 부터지?:2", "성나현:7시라고 했던 것 같은데.:3", "진성후 : 그래도 1군은 아침이라 좋네. 신인 팔 때는 새벽 3, 4시에 사전 녹화 시작이라 잠도 못 자고 막차 타고 와서 노숙했는데.:4", "성나현:난 차라리 3시가 좋았던 것 같은데.:3", "진하연:네가 망돌컬렉터라 맨날 그때 사녹가서 생체리듬이 망돌에 맞춰져 있어서 그래.:2", "성나현:싸울래?:3", "진성후:그러고 보니 네가 웬일로 남돌 사녹에 다 오냐? 너 원래 2-3군 여돌만 팠잖아.:4", "진하연:그러고 보니 얼마 전까지만 해도 무슨 여돌 팬싸 갔다왔다 하지 않았어? 거기 메보 좋아한다며. 이름이...주이나였나?:2", "성나현:오늘은 대리 뛰러 온 거야. 저번에 팬싸 가느라 거지 돼서. 이나는... 하....:3", "진성후:주이나면.... 그, 오르빗?:4", "진하연:오르빗이면 중국 멤은 동북공정하고 일본 멤은 광복절에 일장기 스토리에 올리고, 비주얼 센터가 학폭 해서 개망한 걸그룹 아냐?:2", "성나현:맞으니까 조용히 해라. 안 그래도 심란하니까.:3", "진성후:저걸 한 그룹에서 다 하기도 쉽지 않은데. 병크때문에 탈덕한거야?:4", "성나현:탈덕 안 했거든? 안 그래도 시끄러운데 최애가 이번에 교통사고 나서 활동 중지 중이라 싱숭생숭해서 돈 벌러 온거야. 우리 애 솔로 데뷔하면 앨범 사야 하니까.:3", "진하연:이 난리가 났는데도 솔로 데뷔할 거라고 행복회로 돌리다니. 망돌 아무나 파는 거 아니구나.:2", "성나현:조용히 해라.:3" });

        //오브젝트 대사
        talkData.Add(100, new string[] { "평범한 문. 현재는 닫혀서 열리지 않는다." });
        talkData.Add(107, new string[] { "미처 지우지 못한 회의 내용이 남아있는 화이트보드. HISTAR라는 서바이벌 걸그룹 오디션 프로그램 기획이 적혀있다." });
        talkData.Add(108, new string[] { "화장할 때 사용하는 거울. 조명이 달려있다." });
        talkData.Add(109, new string[] { "휴식을 취할 때 사용하는 소파. 푹신해 보인다." });
        talkData.Add(110, new string[] { "옷을 갈아입을 때 쓰는 탈의실. 잠겨 열리지 않는다." });
        talkData.Add(111, new string[] { "홀로 앉아 휴대전화를 하는 팬. 핸드폰 화면에는 'Orbit 주이나, 교통사고로 의식 손실. 왼쪽 다리를 크게 다쳐.'라고 적혀있다." });
        talkData.Add(112, new string[] { "화려한 스포트라이트 속에서 왼쪽 다리를 질질 끌며 노래하는 귀신이다. 언뜻 보이는 실루엣을 보면, 머리를 양 갈래로 묶고 있는 것 같다. 근처에 사람이 오자 노래를 멈춘다." });   //귀신 풀이 구현

        //아이템 대사
        talkData.Add(101, new string[] { "이다영 교수님께서 주신 포도. 후숙되어 달달하다." });
        talkData.Add(102, new string[] { "맨 위쪽에 방송국 로고가 박혀있는 사원증. 사원 백하연이라고 적혀있다. 방송국에서 일하는 PD의 것인 듯하다." });
        talkData.Add(103, new string[] { "미처 끄지 않아 대기상태에 들어가 있는 컴퓨터. 인터넷 기사 창이 하나 떠 있다." });
        talkData.Add(104, new string[] { "필요 없는 물건을 버리는 쓰레기통. 앨범이 하나 버려져 있다.", "앨범을 펼쳐보면, '주이나'라는 멤버의 사진과 자필 편지가 적혀있는 페이지가 눈에 들어온다." });
        talkData.Add(105, new string[] { "공연 티켓을 판매/배부하는 부스. 아래쪽에 열쇠가 하나 떨어져 있다." });
        talkData.Add(106, new string[] { "보안을 관리하는 경비실. 쪽지가 하나 붙어있다.", "\"신인 아이돌 그룹들이 주로 사전녹화를 하는 3~4시경 귀신 목격담이 많음. 주의할 것.\"" });


        //포트릿 추가가
        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);
        portraitData.Add(1000 + 2, portraitArr[2]);
        portraitData.Add(1000 + 3, portraitArr[3]);
        portraitData.Add(1000 + 4, portraitArr[4]);
        portraitData.Add(1000 + 5, portraitArr[5]);
        portraitData.Add(1000 + 6, portraitArr[6]);
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }
}
