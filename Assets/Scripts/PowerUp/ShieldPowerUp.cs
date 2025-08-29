using UnityEngine;

public class ShieldPowerUp : PowerUp
{
    [SerializeField] private float shieldDuration = 5f;

    protected override void Activate()
    {
        Debug.Log("Shield activated!");
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.ActivateShield(shieldDuration);
        }
    }
}
