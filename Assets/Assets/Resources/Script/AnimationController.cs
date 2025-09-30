using UnityEngine;

public class AnimationController : MonoBehaviour
{
    //외부의 입력을 받아서 애니메이션의 현재 상태를 관리
    //애니메이션의 상태는 애니메이션 변수 'AnimState'에 의해 관리됨
    //'AnimState'의 값에 따라 현재 애니메이션 상태를 결정짓는 방식

    Animator anim;

    void Awake()
    {
        //애니메이션 컴포넌트 초기화
        anim = GetComponent<Animator>();
    }

    public void SetAnimState(int value)
    {
        //애니메이션의 상태를 결정
        //외부 스크립트에서의 사용법
            //AnimationController를 가지는 게임오브젝트.GetComponent<AnimationController>().SetAnimState(설정하고 싶은 값);
        anim.SetInteger("AnimState", value);
    }
}
