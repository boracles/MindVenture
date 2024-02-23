using BitSplash.AI.GPT;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BitSplash.AI.GPT.Extras
{
    public class NPCFriend : MonoBehaviour
    {
        public TMP_InputField QuestionField;
        public GameObject userBalloonPrefab; // 사용자 말풍선 프리팹 추가
        public GameObject chatbotBalloonPrefab; // 챗 지피티 응답용 말풍선 프리팹
        private Transform messagesParent; // 메시지가 생성될 부모 오브젝트
        public Button SubmitButton;

        public string NpcDirection = "CBT기반 상담사로서 소크라테스식 문답법을 사용하여 상담하기";
        public string[] Facts;
        public bool TrackConversation = false;
        public int MaximumTokens = 600;
        [Range(0f, 1f)]
        public float Temperature = 0f;
        ChatGPTConversation Conversation;

        void Start()
        {
            SetUpConversation();
            messagesParent = GameObject.Find("Canvas/Panel/Panel_Chat/Viewport/FrameGroup").transform;
            
            // 앱 시작 시 챗봇으로부터 초기 인사 메시지를 보내는 로직
            SendInitialGreeting();
        }
        
        void SendInitialGreeting()
        {
            // 챗봇의 초기 인사 메시지 내용
            string initialGreeting = "안녕! 나는 '마음솔'이라고해. 만나서 반가워.";

            // 초기 인사 메시지를 대화창에 추가
            CreateMessageBalloon(initialGreeting, false); // false는 메시지가 챗봇으로부터 왔음을 나타냅니다.
        }
        
        void SetUpConversation()
        {
            Conversation = ChatGPTConversation.Start(this)
                .MaximumLength(MaximumTokens)
                .SaveHistory(TrackConversation)
                .System(string.Join("\n", Facts) + "\n" + NpcDirection);
            Conversation.Temperature = Temperature;
        }
        public void SendClicked()
        {
            string userMessage = QuestionField.text;
            if (!string.IsNullOrEmpty(userMessage))
            {
                CreateMessageBalloon(userMessage, true); // 사용자 질문 말풍선 생성
                Conversation.Say(userMessage);
                QuestionField.text = ""; // 필드 초기화
            }
            SubmitButton.interactable = false;
        }

        void OnConversationResponse(string text)
        {
            CreateMessageBalloon(text, false); // 챗 지피티 응답 말풍선 생성
            SubmitButton.interactable = true;
        }
        
        void OnConversationError(string text)
        {
            Debug.Log("Error : " + text);
            Conversation.RestartConversation();
            SubmitButton.interactable = true;
        }

        // 말풍선 생성 메서드
        void CreateMessageBalloon(string message, bool isUserMessage)
        {
            GameObject balloonPrefab = isUserMessage ? userBalloonPrefab : chatbotBalloonPrefab;
            Transform parentTransform = messagesParent;

            GameObject balloonInstance = Instantiate(balloonPrefab, parentTransform);
            TMP_Text messageText;

            if (isUserMessage)
            {
                // 사용자 메시지 프리팹의 경우
                messageText = balloonInstance.transform.Find("Frame_Me/Frame_MyText/Text_AnswerMine").GetComponent<TMP_Text>();
            }
            else
            {
                // 챗봇 메시지 프리팹의 경우
                messageText = balloonInstance.transform.Find("Frame_Chatbot/Frame_ChatbotText/Text_AnswerChatbot").GetComponent<TMP_Text>();
            }

            messageText.text = message;
            
            // 말풍선 생성 로직...
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentTransform as RectTransform);
        }
        
        private void OnValidate()
        {
            SetUpConversation();
        }
    }

}