using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour
{
    private float speed;
    private GameManager gameManager;
    private GameObject shadow;
    private float warningTime;

    public void Initialize(float fallSpeed, GameManager gm, GameObject shadowObj, float delay)
    {
        speed = fallSpeed;
        gameManager = gm;
        shadow = shadowObj;
        warningTime = delay;

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        StartCoroutine(FallRoutine());
    }


    private IEnumerator FallRoutine()
    {
        yield return new WaitForSeconds(warningTime);

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = true;

        while (transform.position.y > -5.2313f)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;

            if (shadow != null && transform.position.y <= -5.0f)
            {
                Destroy(shadow);
                shadow = null;
            }

            yield return null;
        }

        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null && player.IsShieldActive)
            {
                Debug.Log("Meteor blocked by shield!");
                Destroy(gameObject);
                if (shadow != null) Destroy(shadow);
                return;
            }

            gameManager.Die("Hit by Meteor");
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
            if (shadow != null) Destroy(shadow);
        }
    }
}
