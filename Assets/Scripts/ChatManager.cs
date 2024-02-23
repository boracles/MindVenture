using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public GameObject chatbotBalloonPrefab; // 챗봇 메시지용 말풍선 프리팹
    public GameObject userBalloonPrefab; // 사용자 메시지용 말풍선 프리팹
    private Transform chatbotMessagesParent; // 챗봇 메시지가 생성될 부모 오브젝트
    private Transform userMessagesParent; // 사용자 메시지가 생성될 부모 오브젝트

    void Start()
    {
        // 부모 오브젝트 찾기
        chatbotMessagesParent = GameObject.Find("Canvas/Panel/Panel_Chat/FrameGroup_Chatbot").transform;
        userMessagesParent = GameObject.Find("Canvas/Panel/Panel_Chat/FrameGroup_Me").transform;
    }

    // 메시지를 위한 말풍선 생성 및 추가
    public void CreateMessageBalloon(string message, bool isUserMessage)
    {
        GameObject balloonPrefab = isUserMessage ? userBalloonPrefab : chatbotBalloonPrefab;
        Transform parentTransform = isUserMessage ? userMessagesParent : chatbotMessagesParent;

        GameObject balloonInstance = Instantiate(balloonPrefab, parentTransform);
        TMP_Text messageText = balloonInstance.GetComponentInChildren<TMP_Text>();
        messageText.text = message;
    }

    // 사용자가 메시지를 보냈을 때 호출될 수 있는 메서드 예시
    public void OnUserSendMessage(string message)
    {
        CreateMessageBalloon(message, true);
        // 챗 지피티 API와의 통신 로직을 여기에 구현
    }

    // 챗 지피티로부터 응답을 받았을 때 호출될 수 있는 메서드 예시
    public void OnChatGPTResponseReceived(string response)
    {
        CreateMessageBalloon(response, false);
    }
}
