using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] protected string powerupName;
    [SerializeField] protected Sprite icon;

    protected abstract void Activate();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            PowerUpData data = new PowerUpData
            {
                name = powerupName,
                icon = icon,
                Use = Activate
            };

            if (InventoryManager.instance.AddItem(data))
            {
                Destroy(gameObject);
            }
        }
    }
}
