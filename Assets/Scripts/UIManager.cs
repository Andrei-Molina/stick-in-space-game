using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameplayScreen;
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settingScreen;
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private AudioManager audioManager;

    private bool inGame = false;
    private bool isPaused = false;

    public bool InGame => inGame;

    public static UIManager instance { get; private set; }

    private string mainBGM = "";

    private void Start()
    {
        ClearUI();

        if (GameState.StartInGameplay)
        {
            gameplayScreen.SetActive(true);
            inGame = true;
            Time.timeScale = 1f;
            mainBGM = "Space Main Theme";
        }
        else
        {
            mainMenuScreen.SetActive(true);
            inGame = false;
            Time.timeScale = 0f;
            mainBGM = "Exploring the new planets";
        }

        audioManager.PlayMusic(mainBGM);
        GameState.StartInGameplay = false;
    }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (inGame)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    pauseScreen.SetActive(false);
                    Time.timeScale = 1.0f;
                    isPaused = !isPaused;
                }
                else
                {
                    pauseScreen.SetActive(true);
                    Time.timeScale = 0f;
                    isPaused = !isPaused;
                }
            }
        }
    }

    private void ClearUI()
    {
        gameplayScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        pauseScreen.SetActive(false);
        settingScreen.SetActive(false);
    }

    public void PlayGame()
    {
        GameState.StartInGameplay = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenuScreen()
    {
        GameState.StartInGameplay = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SettingScreen()
    {
        ClearUI();
        settingScreen.SetActive(true);
    }

    public void GameOverScreen()
    {
        Time.timeScale = 0f;
        ClearUI();
        gameplayScreen.SetActive(true);
        gameOverScreen.SetActive(true);
    }
}
public static class GameState
{
    public static bool StartInGameplay = false;
}

