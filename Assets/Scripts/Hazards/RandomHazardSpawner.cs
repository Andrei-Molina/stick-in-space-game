using UnityEngine;

public class RandomHazardSpawner : MonoBehaviour
{
    [System.Serializable]
    public class HazardType
    {
        public string name;
        public GameObject triggerPrefab;
        [Range(0f, 1f)] public float weight = 1f;
    }

    [Header("Spawn Settings")]
    [SerializeField] private float spawnChance = 0.1f;
    [SerializeField] private float checkInterval = 2f;

    [Header("Hazard Types")]
    [SerializeField] private HazardType[] hazardTypes;

    [Header("Spawn Area")]
    [SerializeField] private Transform player;
    [SerializeField] private float spawnDistance = 10f;

    [Header("Parent Object")]
    [SerializeField] private Transform gameplayScreen;

    private void Start()
    {
        InvokeRepeating(nameof(CheckForSpawn), checkInterval, checkInterval);
    }

    private void CheckForSpawn()
    {
        if (Random.value <= spawnChance)
        {
            SpawnRandomHazard();
        }
    }

    private void SpawnRandomHazard()
    {
        if (hazardTypes.Length == 0 || player == null) return;

        GameObject selectedHazard = SelectRandomHazard();

        if (selectedHazard != null)
        {
            Vector3 spawnPosition = player.position + Vector3.right * spawnDistance;
            GameObject trigger = Instantiate(selectedHazard, spawnPosition, Quaternion.identity);

            if (gameplayScreen != null)
            {
                trigger.transform.SetParent(gameplayScreen);
            }
        }
    }

    private GameObject SelectRandomHazard()
    {
        float totalWeight = 0f;
        foreach (var hazard in hazardTypes)
        {
            if (hazard.triggerPrefab != null)
                totalWeight += hazard.weight;
        }

        if (totalWeight <= 0f) return null;

        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var hazard in hazardTypes)
        {
            if (hazard.triggerPrefab != null)
            {
                currentWeight += hazard.weight;
                if (randomValue <= currentWeight)
                {
                    return hazard.triggerPrefab;
                }
            }
        }

        return hazardTypes[0].triggerPrefab;
    }
}