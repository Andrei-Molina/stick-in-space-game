using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    [SerializeField] private GameEvent[] randomEvents;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerController player;

    [Range(0f, 1f)][SerializeField] private float randomEventChance = 0.2f;

    [SerializeField] private DistanceManager distanceManager;
    [SerializeField] private float checkInterval = 10f;
    private float nextCheckDistance = 10f;

    void Update()
    {
        if (distanceManager.Distance >= nextCheckDistance)
        {
            TryTriggerEvent();
            nextCheckDistance += checkInterval;
        }
    }

    public void TryTriggerEvent()
    {
        float roll = Random.value;
        if (roll < randomEventChance && randomEvents.Length > 0)
        {
            int index = Random.Range(0, randomEvents.Length);
            randomEvents[index].Trigger(gameManager, player);
        }
    }
}
