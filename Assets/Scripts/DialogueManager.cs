using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro; // TextMeshPro를 사용할 경우 추가

public class DialogueManager : MonoBehaviour
{
    public string savePath = "DialogueSave.txt";
    private List<string> dialogueHistory = new List<string>(); // 대화 내용을 저장할 리스트

    // 말풍선 텍스트를 대화 내용 리스트에 추가하는 메서드
    public void AddDialogue(string message)
    {
        dialogueHistory.Add(message);
    }

    // 대화 종료 및 내용 저장
    public void EndDialogueAndSave()
    {
        // 리스트에 저장된 대화 내용을 하나의 문자열로 결합
        string dialogueContent = string.Join("\n", dialogueHistory);

        // 파일 저장
        SaveDialogueContent(dialogueContent);

        // 앱 종료
        Application.Quit();
    }

    // 대화 내용을 파일에 저장
    public void SaveDialogueContent(string content)
    {
        string fullPath = Path.Combine(Application.persistentDataPath, savePath);
        File.WriteAllText(fullPath, content);
    }
}