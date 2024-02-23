using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatBalloonSizeAdjuster : MonoBehaviour
{
    public RectTransform balloonRectTransform; // 말풍선의 RectTransform
    public TextMeshProUGUI textMeshPro; // 텍스트 컴포넌트
    public float minHeight = 100f; // 말풍선의 최소 세로 길이
    public float additionalVerticalPadding = 60.0f; // 세로 여백을 더 넉넉하게 조정
    
    private ScrollRect scrollRect; // 스크롤 뷰를 조작하기 위한 ScrollRect 변수 선언
    
    void Start()
    {
        // ScrollRect 컴포넌트를 찾아 할당
        scrollRect = GameObject.Find("Canvas/Panel/Panel_Chat").GetComponent<ScrollRect>();
    }
    
    void OnEnable()
    {
        // 프리팹이 활성화될 때 스크롤을 맨 아래로 이동
        StartCoroutine(ScrollToBottom());
    }

    void Update()
    {
        AdjustBalloonHeightIfNeeded();
    }
    
    void AdjustParentHeight()
    {
        // 부모 RectTransform의 참조를 얻습니다.
        RectTransform parentRectTransform = balloonRectTransform.parent.GetComponent<RectTransform>();
        RectTransform grandparentRectTransform = parentRectTransform.parent.GetComponent<RectTransform>();

        // 현재 부모의 높이를 계산합니다. 여기서는 간단히 말풍선의 높이에 추가 여백을 더한 값으로 설정할 수 있습니다.
        float updatedParentHeight = balloonRectTransform.sizeDelta.y + additionalVerticalPadding; // 추가 여백을 포함한 새로운 높이
        float updatedGrandparentHeight = updatedParentHeight + additionalVerticalPadding; // 할아버지 오브젝트에도 동일한 로직 적용

        // 부모 오브젝트들의 높이만 조정합니다. 너비는 1200으로 고정.
        parentRectTransform.sizeDelta = new Vector2(1200, Mathf.Max(parentRectTransform.sizeDelta.y, updatedParentHeight));
        grandparentRectTransform.sizeDelta = new Vector2(1200, Mathf.Max(grandparentRectTransform.sizeDelta.y, updatedGrandparentHeight));
    }
    void AdjustBalloonHeightIfNeeded()
    {
        // 말풍선 크기 조정 로직...
        if (textMeshPro == null || balloonRectTransform == null)
        {
            Debug.LogError("One or more required components are missing.");
            return;
        }

        // 텍스트의 선호 높이와 가로 길이 계산
        float preferredHeight = textMeshPro.preferredHeight + additionalVerticalPadding;
        float newHeight = Mathf.Max(preferredHeight, minHeight);
        float preferredWidth = textMeshPro.preferredWidth + additionalVerticalPadding; // 가로 여백을 고려한 선호 가로 길이
        float newWidth = Mathf.Clamp(preferredWidth, 140f, 770f); // 가로 길이를 최소 140, 최대 770으로 제한

        // 말풍선의 sizeDelta 조정하여 높이와 가로 길이 변경
        balloonRectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        
        // 부모 오브젝트들의 높이 조정
        AdjustParentHeight();
    }
    
    IEnumerator ScrollToBottom()
    {
        // UI 레이아웃 업데이트를 기다립니다.
        yield return new WaitForSeconds(0.1f);

        if (scrollRect != null)
        {
            // ScrollRect의 verticalNormalizedPosition을 0으로 설정하여 스크롤을 맨 아래로 이동
            scrollRect.verticalNormalizedPosition = 0;
        }
    }
}