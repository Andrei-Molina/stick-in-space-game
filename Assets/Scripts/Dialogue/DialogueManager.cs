using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image characterPortrait;
    private System.Action onEndCallback;

    private DialogueLine[] lines;
    private int currentLine;
    private bool dialogueActive;

    private Rigidbody2D playerRb;
    private RigidbodyConstraints2D originalConstraints;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (dialogueActive && Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }

    public void StartDialogue(DialogueData data, System.Action onEnd = null)
    {
        if (playerRb == null)
        {
            playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            originalConstraints = playerRb.constraints;
        }

        playerRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        lines = data.lines;
        currentLine = 0;
        dialogueActive = true;
        dialogueCanvas.SetActive(true);
        onEndCallback = onEnd;
        ShowLine();
    }

    public void NextLine()
    {
        currentLine++;
        if (currentLine < lines.Length)
        {
            ShowLine();
        }

        else
        {
            EndDialogue();
        }
    }

    private void ShowLine()
    {
        nameText.text = lines[currentLine].speakerName;
        dialogueText.text = lines[currentLine].text;
        characterPortrait.sprite = lines[currentLine].sprite;
    }

    private void EndDialogue()
    {
        playerRb.constraints = originalConstraints;

        dialogueActive = false;
        dialogueCanvas.SetActive(false);

        onEndCallback?.Invoke();
        onEndCallback = null;
    }
}

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 5)]
    public string text;
    public Sprite sprite;
}
