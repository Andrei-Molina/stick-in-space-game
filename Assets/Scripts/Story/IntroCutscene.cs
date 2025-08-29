using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameObject ashCharacter;

    private void OnEnable()
    {
        dialogueManager.OnDialogueFinished += HandleDialogueEnd;
    }

    private void OnDisable()
    {
        dialogueManager.OnDialogueFinished -= HandleDialogueEnd;
    }

    private void HandleDialogueEnd()
    {
        if (ashCharacter != null)
        {
            Animator anim = ashCharacter.GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger("FlyAway");

            Destroy(ashCharacter, 2f);
        }

        Debug.Log("Cutscene finished. Start game");
    }
}
