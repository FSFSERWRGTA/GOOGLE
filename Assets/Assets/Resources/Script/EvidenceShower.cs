using UnityEngine;
using UnityEngine.UI;

public class EvidenceShower : MonoBehaviour
{
    //증거 이미지를 보여주는 스크립트
    Image image;            //출력할 이미지

    //이미지를 설정하고 바로 보여줌
    public void ShowEvidenceImg(Sprite spr)
    {
        if (image == null) image = GetComponent<Image>();
        image.sprite = spr;
        gameObject.SetActive(true);
    }

    //이미지를 닫음
    public void ExitEvidenceImg()
    {
        gameObject.SetActive(false);
    }
}
