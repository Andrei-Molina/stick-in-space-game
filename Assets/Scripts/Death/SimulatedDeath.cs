using UnityEngine;

public class SimulatedDeath : MonoBehaviour
{
    [SerializeField] private KeyCode deathKey = KeyCode.K;
    [SerializeField] private GameManager gameManager;

    private void Update()
    {
        if (Input.GetKeyDown(deathKey))
        {
            gameManager.Die("SimulatedHazard");
        }
    }
}
