using System.Collections;
using UnityEngine;

public class MeteorStrikeHandler : MonoBehaviour
{
    private GameObject meteorShadow;
    private Vector3 targetPosition;
    private bool hasHitGround = false;

    [Header("Meteor Settings")]
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private float shadowGrowthDuration = 2f;

    private readonly Vector3 finalShadowScale = new Vector3(103.6346f, 40.80102f, 40.80102f);
    private readonly Vector3 fixedShadowRotation = new Vector3(105.635f, 0f, 0f);

    public void Initialize(GameObject shadow, Vector3 target)
    {
        meteorShadow = shadow;
        targetPosition = target;

        if (meteorShadow != null)
        {
            meteorShadow.transform.rotation = Quaternion.Euler(fixedShadowRotation);
            meteorShadow.transform.localScale = finalShadowScale;

            StartCoroutine(RemoveShadowAfterDelay());
        }

        StartCoroutine(FallToTarget());
    }

    private IEnumerator RemoveShadowAfterDelay()
    {
        yield return new WaitForSeconds(shadowGrowthDuration);

        if (meteorShadow != null)
        {
            Destroy(meteorShadow);
        }
    }

    private IEnumerator FallToTarget()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f && !hasHitGround)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);
            yield return null;
        }

        hasHitGround = true;
        OnHitGround();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && player.IsShieldActive())
            {
                Destroy(gameObject);
                return;
            }

            Destroy(gameObject);
            UIManager.instance.GameOverScreen();

            FlagDropper dropper = FindObjectOfType<FlagDropper>();
            if (dropper != null)
            {
                dropper.PlayerDied();
            }
        }
        else if (collision.CompareTag("Ground"))
        {
            OnHitGround();
        }
    }

    private void OnHitGround()
    {
        if (!hasHitGround)
        {
            hasHitGround = true;
            Destroy(gameObject);
        }
    }
}