using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("Inventory Slots (UI Images)")]
    [SerializeField] private Image[] slots = new Image[5];

    private PowerUpData[] items = new PowerUpData[5];

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Update()
    {
        // Keys 1-5
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) UseItem(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) UseItem(4);
    }

    public bool AddItem(PowerUpData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = data;
                slots[i].sprite = data.icon;
                slots[i].enabled = true;
                return true;
            }
        }
        return false; // inventory full
    }

    private void UseItem(int index)
    {
        if (items[index] != null)
        {
            items[index].Use();
            items[index] = null;
            slots[index].sprite = null;
            slots[index].enabled = false;
        }
    }
}
