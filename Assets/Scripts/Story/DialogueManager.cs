using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialogueSequence sequence;

    private int currentIndex = 0;
    private bool isDialogueActive = false;

    public event Action OnDialogueFinished;

    private void Start()
    {
        dialogueUI.Hide();
    }

    private void Update()
    {
        if (!isDialogueActive) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }
    public void StartDialogue()
    {
        if (sequence == null || sequence.lines.Length == 0) return;

        currentIndex = 0;
        isDialogueActive = true;

        dialogueUI.Show();
        ShowLine(currentIndex);
    }

    private void ShowLine(int index)
    {
        if (index < sequence.lines.Length)
        {
            dialogueUI.DisplayLine(sequence.lines[index]);
        }
        else
        {
            EndDialogue();
        }
    }

    private void NextLine()
    {
        currentIndex++;
        ShowLine(currentIndex);
    }

    private void EndDialogue()
    {
        dialogueUI.Hide();
        OnDialogueFinished?.Invoke();
    }
}
