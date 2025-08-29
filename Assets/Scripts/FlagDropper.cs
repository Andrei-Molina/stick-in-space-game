using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlagDropper : MonoBehaviour
{
    [SerializeField] private GameObject flag;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private DistanceManager distanceManager;

    private string rating;

    [Header("Game Over UI")]
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private TextMeshProUGUI currentDistanceText;
    [SerializeField] private TextMeshProUGUI bestDistanceText;

    [Header("Cutscene Settings")]
    [SerializeField] private GameObject cookiePrefab;
    [SerializeField] private GameObject cookieDPrefab;
    [SerializeField] private Transform cookieSpawnPoint;
    [SerializeField] private CinemachineVirtualCamera cookieECinemachineCamera;
    [SerializeField] private CinemachineVirtualCamera rankECinemachineCamera;

    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite sadSprite;
    [SerializeField] private GameObject smile;

    [SerializeField] private GameObject inventorySlots;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private bool isGrounded;
    private bool isFlagDropped = false;

    private const string BestDistanceKey = "BestDistance";

    void Update()
    {
        if (flag == null || playerTransform == null) return;

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        if (isGrounded && !isFlagDropped && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(DropFlagSequence());
            isFlagDropped = true;
        }
    }

    private IEnumerator DropFlagSequence()
    {
        Rigidbody2D rbPlayer = playerTransform.GetComponent<Rigidbody2D>();
        if (rbPlayer != null)
        {
            rbPlayer.velocity = Vector2.zero;
            rbPlayer.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        Instantiate(flag, new Vector3(playerTransform.position.x + 3, -5.2313f, playerTransform.position.z), Quaternion.identity);
        RecordDistance();
        SaveBestDistance();

        if (rating == "E")
        {
            inventorySlots.gameObject.SetActive(false);

            Vector3 offset = new Vector3(0.7164f, 5f, 0);
            GameObject cookie = Instantiate(cookiePrefab, playerTransform.position + offset, Quaternion.identity);
            cookieECinemachineCamera.gameObject.SetActive(true);
            while (cookie.transform.position.y > -5.9f)
            {
                yield return null;
            }

            Rigidbody2D rb = cookie.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            yield return new WaitForSeconds(3f);

            playerSpriteRenderer.sprite = sadSprite;

            cookieECinemachineCamera.gameObject.SetActive(false);

            rankECinemachineCamera.gameObject.SetActive(true);

            yield return new WaitForSeconds(3f);
        }

        if (rating == "D")
        {
            inventorySlots.gameObject.SetActive(false);
            smile.gameObject.SetActive(true);

            Vector3 offset = new Vector3(36.2624f, 15.6313f, 0);
            GameObject cookie = Instantiate(cookieDPrefab, playerTransform.position + offset, Quaternion.identity);

            yield return new WaitForSeconds(5f);

            float duration = 1f;
            float elapsed = 0f;
            Vector3 startScale = Vector3.zero;
            Vector3 targetScale = new Vector3(0.2f, 0.2f, 0.2f);

            smile.transform.localScale = startScale;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                smile.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
                yield return null;
            }

            smile.transform.localScale = targetScale;

            yield return new WaitForSeconds(2f);
        }

        UIManager.instance.GameOverScreen();

        ratingText.text = $"Rating: {rating}";
        currentDistanceText.text = $"Distance: {distanceManager.Distance.ToString("F2")}m";
        bestDistanceText.text = PlayerPrefs.HasKey(BestDistanceKey)
            ? $"Best Distance: {PlayerPrefs.GetFloat(BestDistanceKey).ToString("F2")}m"
            : "Best Distance: -";
    }

    private void RecordDistance()
    {
        switch(distanceManager.Distance)
        {
            case < 500:
                rating = "E";
                break;
            case < 1000:
                rating = "D";
                break;
            case < 1500:
                rating = "C";
                break;
            case < 2000:
                rating = "B";
                break;
            case < 2500:
                rating = "A";
                break;
        }
    }

    private void SaveBestDistance()
    {
        float bestDistance = PlayerPrefs.GetFloat(BestDistanceKey, 0);

        if (distanceManager.Distance > bestDistance)
        {
            PlayerPrefs.SetFloat(BestDistanceKey, distanceManager.Distance);
            PlayerPrefs.Save();
        }
    }

    public void PlayerDied()
    {
        UIManager.instance.GameOverScreen();

        ratingText.text = "You died :(";
        currentDistanceText.text = "Distance: -";
        if (PlayerPrefs.HasKey(BestDistanceKey))
        {
            bestDistanceText.text = $"Best Distance: {PlayerPrefs.GetFloat(BestDistanceKey)}m";
        }
        else
        {
            bestDistanceText.text = "Best Distance: -";
        }
    }
}
