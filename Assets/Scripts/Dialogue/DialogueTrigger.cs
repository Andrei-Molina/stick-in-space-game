using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseTrigger;

public class DialogueTrigger : BaseTrigger
{
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private GameObject triggerObject;
    [SerializeField] private bool playAnimationAfterDialogue = false;
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName;

    [SerializeField] private bool destroyAfterDialogue = false;
    [SerializeField] private float destroyDelay = 2f;

    protected override void OnTriggered()
    {
        DialogueManager.instance.StartDialogue(dialogueData, () =>
        {
            if (playAnimationAfterDialogue && animator != null && !string.IsNullOrEmpty(triggerName))
            {
                animator.SetTrigger(triggerName);
            }

            if (destroyAfterDialogue)
            {
                Destroy(gameObject, destroyDelay);
            }
        });


        switch (action)
        {
            case TriggerEndAction.Disable:
                (triggerObject ?? gameObject).SetActive(false);
                break;
            case TriggerEndAction.Destroy:
                Destroy(triggerObject ?? gameObject);
                break;
        }

    }
}


