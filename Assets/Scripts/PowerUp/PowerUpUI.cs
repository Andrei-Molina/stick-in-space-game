using UnityEngine;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour
{
    public Image[] slotImages;

    public void UpdateUI(PowerUp[] slots)
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (slots[i] != null && slots[i].icon != null)
            {
                slotImages[i].sprite = slots[i].icon;
                slotImages[i].enabled = true;
            }
            else
            {
                slotImages[i].sprite = null;
                slotImages[i].enabled = false;
            }
        }
    }
}
