using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public PowerUp[] slots = new PowerUp[5];
    public PowerUpUI ui;

    private GameManager gameManager;
    private PlayerController player;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<PlayerController>();
        ui.UpdateUI(slots);
    }

    void Update()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                if (slots[i] != null)
                {
                    slots[i].Use(gameManager, player);
                    ui.UpdateUI(slots);
                }
            }
        }
    }

    public bool AddPowerUp(PowerUp powerUp)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = powerUp;
                ui.UpdateUI(slots);
                return true;
            }
        }
        return false; // hotbar full
    }

    public void ClearSlotContaining(PowerUp powerUp)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == powerUp)
            {
                slots[i] = null;
                ui.UpdateUI(slots);
                return;
            }
        }
    }
}
