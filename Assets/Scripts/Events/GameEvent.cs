using UnityEngine;

public abstract class GameEvent : ScriptableObject
{
    public string eventName;
    public abstract void Trigger(GameManager gameManager, PlayerController player);
}
