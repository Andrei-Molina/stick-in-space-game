using UnityEngine;

public class FlagDropper : MonoBehaviour, IFlagDropper
{
    [SerializeField] private GameObject flagPrefab;

    public void DropFlag(Vector3 position)
    {
        if (flagPrefab == null)
        {
            Debug.LogError("FlagDropper: No flag prefab assigned!");
            return;
        }

        Instantiate(flagPrefab, position, Quaternion.identity);
    }
}
