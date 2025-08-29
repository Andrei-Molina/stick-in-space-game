using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    public PowerUp powerUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PowerUpManager manager = FindObjectOfType<PowerUpManager>();
            if (manager.AddPowerUp(powerUp))
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Hotbar full!");
            }
        }
    }
}
