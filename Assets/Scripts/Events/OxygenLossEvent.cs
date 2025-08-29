using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

[CreateAssetMenu(menuName = "GameEvents/OxygenLoss")]
public class OxygenLossEvent : GameEvent
{
    [SerializeField] private float duration = 5f; // time until player dies

    public override void Trigger(GameManager gameManager, PlayerController player)
    {
        player.oxygenCoroutine = player.StartCoroutine(OxygenRoutine(player, gameManager));
    }

    public IEnumerator OxygenRoutine(PlayerController player, GameManager gameManager)
    {
        player.ResetOxygenLossFlag();

        float elapsed = 0f;
        Light2D light = player.GetComponentInChildren<Light2D>();
        SpriteRenderer sprite = player.GetComponent<SpriteRenderer>();

        Color startLight = light != null ? light.color : Color.white;
        Color endLight = Color.blue;

        Color startSprite = sprite != null ? sprite.color : Color.white;
        Color endSprite = Color.blue;

        while (elapsed < duration)
        {
            if (player.IsOxygenLossCancelled)
                yield break;

            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (light != null)
                light.color = Color.Lerp(startLight, endLight, t);

            if (sprite != null)
                sprite.color = Color.Lerp(startSprite, endSprite, t);

            yield return null;
        }

        if (!player.IsOxygenLossCancelled)
            yield return DeathFallRoutine(player, gameManager);
    }



    private IEnumerator DeathFallRoutine(PlayerController player, GameManager gameManager)
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

        gameManager.Die("Lost Oxygen");
    }
}
