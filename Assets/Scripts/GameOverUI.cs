using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI explorationRatingText;
    [SerializeField] private TextMeshProUGUI currentDistanceText;
    [SerializeField] private TextMeshProUGUI bestDistanceText;

    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        panel.SetActive(false);
    }

    public void ShowGameOver(string rating, float? currentDistance, float? bestDistance)
    {
        panel.SetActive(true);

        explorationRatingText.text = $"Exploration Rating: {rating}";

        if (currentDistance.HasValue)
            currentDistanceText.text = $"Current Distance {currentDistance.Value:F1}";
        else
            currentDistanceText.text = $"Current Distance: -";

        if (bestDistance.HasValue)
            bestDistanceText.text = $"Best Distance {bestDistance.Value:F1}";
        else
            bestDistanceText.text = $"Best Distance: -";

        Time.timeScale = 0;
    }

    public void OnPlayAgain()
    {
        Time.timeScale = 1f;

        PlayerPrefs.SetInt("RestartGame", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
