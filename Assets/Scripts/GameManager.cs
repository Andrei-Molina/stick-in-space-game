using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDeathHandler
{
    [SerializeField] private DistanceManager distanceManager;
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private PowerUpManager powerUpManager;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private BiscuitRewardManager biscuitRewardManager;
    [SerializeField] private AudioManager audioManager;

    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject gameplayScreen;

    private float bestDistance = 0f;
    private float? currentDistance = null; // null = no dropped flag

    private bool isGameOver = false;
    public PowerUpManager PowerUpManager => powerUpManager;
    public DialogueManager DialogueManager => dialogueManager;

    public bool IsInCutscene { get; private set; }

    private void Awake()
    {
        bestDistance = PlayerPrefs.GetFloat("BestDistance", 0f);

        if (PlayerPrefs.GetInt("RestartGame", 0) == 1)
        {
            PlayerPrefs.SetInt("RestartGame", 0);
            PlayerPrefs.Save();

            mainMenuScreen.SetActive(false);
            gameplayScreen.SetActive(true);

            StartCoroutine(RestartWithDialogue());
        }
    }

    private void Start()
    {
        audioManager.PlayMusic("Exploring the new planets");
    }

    public void Die(string cause)
    {
        if (isGameOver) return;
        isGameOver = true;

        if (cause == "FlagDrop")
        {
            currentDistance = distanceManager.Distance;
            if (currentDistance > bestDistance)
            {
                bestDistance = currentDistance.Value;

                PlayerPrefs.SetFloat("BestDistance", bestDistance);
                PlayerPrefs.Save();
            }
                
        }
        else
        {
            currentDistance = null;
        }

        string rating = currentDistance.HasValue ? ScoreManager.GetRank(currentDistance.Value) : "-";
        StartCoroutine(HandleDeathSequence(rating));
    }
    public void StartGame()
    {
        mainMenuScreen.SetActive(false);
        gameplayScreen.SetActive(true);
        audioManager.PlayMusic("Space Main Theme");
        dialogueManager.StartDialogue();
    }

    private IEnumerator RestartWithDialogue()
    {
        yield return null;
        dialogueManager.StartDialogue();
    }

    public void SetCutsceneState(bool active)
    {
        IsInCutscene = active;
    }

    private IEnumerator HandleDeathSequence(string rating)
    {
        yield return StartCoroutine(biscuitRewardManager.PlayCutscene(rating));

        gameOverUI.ShowGameOver(rating, currentDistance, bestDistance);
    }
}
