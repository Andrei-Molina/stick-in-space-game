using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvents/GravityCrush")]
public class GravityCrushEvent : GameEvent
{
    [SerializeField] private float duration = 5f;     // time until player dies
    [SerializeField] private float pressesRequired = 10f; // how many presses should roughly free the player

    public override void Trigger(GameManager gameManager, PlayerController player)
    {
        player.StartCoroutine(CrushRoutine(player, gameManager));
    }

    private IEnumerator CrushRoutine(PlayerController player, GameManager gameManager)
    {
        float elapsed = 0f;
        float resistance = 0f;

        Vector3 startPos = player.transform.position;
        Vector3 endPos = new Vector3(startPos.x, -7.7293f, startPos.z);

        Vector3 startScale = player.transform.localScale;
        Vector3 endScale = new Vector3(startScale.x, 0.2281955f, startScale.z);

        float resistancePerPress = 1f / pressesRequired;

        UIManager.Instance.ShowGravityCrushUI(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                resistance += resistancePerPress;
            }

            float crushProgress = Mathf.Clamp01(t - resistance);

            player.transform.localScale = Vector3.Lerp(startScale, endScale, crushProgress);
            player.transform.position = Vector3.Lerp(startPos, endPos, crushProgress);

            if (crushProgress <= 0f && resistance >= 1f)
            {
                Debug.Log("Player resisted Gravity Crush!");
                UIManager.Instance.ShowGravityCrushUI(false);
                yield break;
            }

            yield return null;
        }

        UIManager.Instance.ShowGravityCrushUI(false);
        if (Vector3.Distance(player.transform.localScale, endScale) < 0.01f)
        {
            gameManager.Die("Crushed by Gravity");
        }
    }
}
