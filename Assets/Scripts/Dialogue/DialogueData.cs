using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines;
}
