using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private Image portraitImage;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI contentText;

    public void DisplayLine(DialogueLine line)
    {
        if (portraitImage != null && line.portrait != null)
            portraitImage.sprite = line.portrait;

        if (speakerNameText != null)
            speakerNameText.text = line.speakerName;

        if (contentText != null)
            contentText.text = line.content;
    }

    public void Hide()
    {
        dialogueUI.SetActive(false);
    }

    public void Show()
    {
        dialogueUI.SetActive(true);
    }
}
