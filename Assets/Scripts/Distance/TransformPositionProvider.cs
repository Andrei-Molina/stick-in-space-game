using UnityEngine;
public class TransformPositionProvider : MonoBehaviour, IPositionProvider
{
    [SerializeField] private Transform target;

    public Vector3 GetPosition()
    {
        return target ? target.position : transform.position;
    }
}
