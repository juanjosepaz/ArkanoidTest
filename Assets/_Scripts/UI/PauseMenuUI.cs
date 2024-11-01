using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuUI : MonoBehaviour
{
    public static bool isGamePaused;
    public static bool canPauseGame;
    public static PauseMenuUI Instance { get; private set; }

    [SerializeField] private GameObject pauseMenuGameObject;
    [SerializeField] private GameObject resumeButton;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameManager.OnGameStarted += EnablePauseGame;
        GameManager.OnGameOver += DisablePauseGame;
        GameManager.OnRoundWon += DisablePauseGame;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= EnablePauseGame;
        GameManager.OnGameOver -= DisablePauseGame;
        GameManager.OnRoundWon -= DisablePauseGame;
    }

    private void EnablePauseGame()
    {
        canPauseGame = true;
    }

    private void DisablePauseGame()
    {
        canPauseGame = false;
    }

    public void PerformePauseButton()
    {
        if (!canPauseGame) { return; }

        if (isGamePaused)
        {
            ResumeGameButton();
        }
        else
        {
            OpenPauseMenu();
        }
    }

    private void PauseGame()
    {
        isGamePaused = true;

        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        isGamePaused = false;

        Time.timeScale = 1f;
    }

    public void OpenPauseMenu()
    {
        pauseMenuGameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(resumeButton);

        PauseGame();

        SoundManager.Instance.PlayTextEnableSound();
    }

    public void ResumeGameButton()
    {
        EventSystem.current.SetSelectedGameObject(null);

        pauseMenuGameObject.SetActive(false);

        ResumeGame();

        SoundManager.Instance.PlayTextEnableSound();
    }


    public void RestartGameButton()
    {
        SoundManager.Instance.PlayTextEnableSound();

        GameplayDataManager.Instance.ResetScoreOnLevelReset();

        EventSystem.current.SetSelectedGameObject(null);

        SceneManagerSingleton.Instance.ReloadScene();
    }

    public void MainMenuGameButton()
    {
        SoundManager.Instance.PlayTextEnableSound();

        EventSystem.current.SetSelectedGameObject(null);

        SceneManagerSingleton.Instance.LoadMainMenu();
    }
}
