using UnityEngine;
using TMPro;

public class DistanceUI : MonoBehaviour
{
    [SerializeField] private DistanceManager distanceManager;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private string prefix = "Distance: ";
    [SerializeField] private int decimals = 0;

    private void OnEnable()
    {
        distanceManager.OnDistanceChanged += UpdateUI;
        UpdateUI(distanceManager.Distance);
    }

    private void OnDisable()
    {
        distanceManager.OnDistanceChanged -= UpdateUI;
    }

    private void UpdateUI(float value)
    {
        if (value < 1000)
            distanceText.text = $"{prefix}{value:F0} m";
        else
            distanceText.text = $"{prefix}{(value / 1000f):F2} km";
    }

}
