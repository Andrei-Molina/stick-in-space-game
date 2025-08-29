using UnityEngine;

public class ShadowController : MonoBehaviour
{
    private Vector3 targetScale;
    private Quaternion targetRotation;
    private float duration;
    private float elapsed = 0f;
    private Vector3 startScale;
    private Quaternion startRotation;

    public void Initialize(Vector3 finalScale, Quaternion fixedRotation, float growDuration)
    {
        targetScale = finalScale;
        targetRotation = fixedRotation;
        duration = growDuration;

        startScale = finalScale * 0.1f;
        startRotation = fixedRotation;

        transform.localScale = startScale;
        transform.rotation = startRotation;
    }

    private void Update()
    {
        if (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
        }
    }
}
