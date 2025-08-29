using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [System.Serializable]
    public class PowerUpType
    {
        public string name;
        public GameObject prefab;
        [Range(0f, 1f)] public float weight = 1f;
    }

    [Header("Spawn Settings")]
    [SerializeField] private float spawnChance = 0.2f;
    [SerializeField] private float checkInterval = 5f;

    [Header("PowerUps")]
    [SerializeField] private PowerUpType[] powerUps;

    [Header("Spawn Area")]
    [SerializeField] private Transform player;
    [SerializeField] private float spawnDistance = 5f;

    [Header("Parent Object")]
    [SerializeField] private Transform gameplayScreen;

    private void Start()
    {
        InvokeRepeating(nameof(CheckForSpawn), checkInterval, checkInterval);
    }

    private void CheckForSpawn()
    {
        if (Random.value <= spawnChance)
            SpawnRandomPowerUp();
    }

    private void SpawnRandomPowerUp()
    {
        if (powerUps.Length == 0 || player == null) return;

        GameObject selected = SelectRandomPowerUp();
        if (selected != null)
        {
            Vector3 spawnPos = new Vector3(player.position.x + spawnDistance, -5.2313f, 0f);
            GameObject go = Instantiate(selected, spawnPos, Quaternion.identity);
            if (gameplayScreen != null)
                go.transform.SetParent(gameplayScreen);
        }
    }

    private GameObject SelectRandomPowerUp()
    {
        float totalWeight = 0f;
        foreach (var p in powerUps) totalWeight += p.weight;

        float rand = Random.Range(0f, totalWeight);
        float current = 0f;

        foreach (var p in powerUps)
        {
            current += p.weight;
            if (rand <= current) return p.prefab;
        }

        return powerUps[0].prefab;
    }
}
