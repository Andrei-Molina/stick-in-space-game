using System;
using UnityEngine;

public class DistanceManager : MonoBehaviour
{
    [SerializeField] private MonoBehaviour positionProviderBehaviour;
    [SerializeField] private bool useXAxis = true;
    [SerializeField] private bool useYAxis = false;

    private IPositionProvider provider;
    private float startValue;
    private float distance;

    public event Action<float> OnDistanceChanged;
    public float Distance => distance;

    private void Awake()
    {
        provider = positionProviderBehaviour as IPositionProvider;
        if (provider == null)
        {
            Debug.LogError("There is no IPositionProvider...");
            enabled = false;
            return;
        }

        var pos = provider.GetPosition();
        startValue = useXAxis ? pos.x : pos.y;
    }

    private void Update()
    {
        var pos = provider.GetPosition();
        float current = useXAxis ? pos.x : pos.y;

        distance = Mathf.Max(0f, current - startValue);
        OnDistanceChanged?.Invoke(distance);
    }

    public void ResetDistance()
    {
        var pos = provider.GetPosition();
        startValue = useXAxis ? pos.x : pos.y;
        distance = 0f;
        OnDistanceChanged?.Invoke(distance);
    }
}
