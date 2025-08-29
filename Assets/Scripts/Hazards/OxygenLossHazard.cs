using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class OxygenLossHazard : MonoBehaviour
{
    [SerializeField] private float duration = 5f;

    private bool isTriggered = false;
    [SerializeField] private AudioManager audioManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioManager = FindObjectOfType<AudioManager>();
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null && !isTriggered)
        {
            isTriggered = true;
            player.StartOxygenLoss(duration);
            audioManager.PlaySFX("Panting");
        }
    }

    public IEnumerator DeathFallRoutine(PlayerController player)
    {
        float fallDuration = 2f;
        float elapsed = 0f;

        Quaternion startRot = player.transform.rotation;
        Quaternion endRot = Quaternion.Euler(0, 0, -90);

        Vector3 startPos = player.transform.position;
        Vector3 endPos = new Vector3(startPos.x, -8.16f, startPos.z);

        while (elapsed < fallDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fallDuration;

            player.transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            player.transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        FlagDropper dropper = FindObjectOfType<FlagDropper>();
        if (dropper != null)
        {
            dropper.PlayerDied();
        }

        UIManager.instance.GameOverScreen();
    }
}
