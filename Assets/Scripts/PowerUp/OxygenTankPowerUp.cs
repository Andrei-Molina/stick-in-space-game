using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OxygenTankPowerUp : PowerUp
{
    protected override void Activate()
    {
        Debug.Log("Oxygen Tank used!");
        PlayerController player = FindObjectOfType<PlayerController>();
        player.CancelOxygenLoss();
    }
}
