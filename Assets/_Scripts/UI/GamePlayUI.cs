using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayUI : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Lifes")]
    [SerializeField] private GameObject lifeImagePrefab;
    [SerializeField] private Transform lifeSpritesContainer;
    [SerializeField] private Stack<GameObject> lifeImageGameObjects = new();

    private void Start()
    {
        InitializeGameplayUI();
    }

    private void InitializeGameplayUI()
    {
        scoreText.text = GameplayDataManager.Instance.GetActualScore().ToString();
        highScoreText.text = GameplayDataManager.Instance.GetActualHighScore().ToString();
        SetLifesOnScreen(GameplayDataManager.Instance.GetActualLifes());
    }

    private void OnEnable()
    {
        GameplayDataManager.OnScoreChanged += GameplayDataManager_OnScoreChanged;
        GameplayDataManager.OnHighScoreChanged += GameplayDataManager_OnHighScore;
        GameplayDataManager.OnLifesChanged += GameplayDataManager_OnLifesChanged;

    }

    private void OnDisable()
    {
        GameplayDataManager.OnScoreChanged -= GameplayDataManager_OnScoreChanged;
        GameplayDataManager.OnHighScoreChanged -= GameplayDataManager_OnHighScore;
        GameplayDataManager.OnLifesChanged -= GameplayDataManager_OnLifesChanged;
    }

    private void GameplayDataManager_OnScoreChanged(int score)
    {
        scoreText.text = score.ToString();
    }

    private void GameplayDataManager_OnHighScore(int highScore)
    {
        highScoreText.text = highScore.ToString();
    }

    private void GameplayDataManager_OnLifesChanged(int lifes)
    {
        SetLifesOnScreen(lifes);
    }

    private void SetLifesOnScreen(int lifes)
    {
        int lifeImagesToSpawn = lifes - lifeImageGameObjects.Count;

        if (lifeImagesToSpawn == 0) { return; }

        if (lifeImagesToSpawn > 0)
        {
            for (int i = 0; i < lifeImagesToSpawn; i++)
            {
                lifeImageGameObjects.Push(Instantiate(lifeImagePrefab, lifeSpritesContainer));
            }
        }
        else
        {
            if (lifeImageGameObjects.Count == 0) { return; }

            GameObject lastImage = lifeImageGameObjects.Pop();
            Destroy(lastImage);
        }
    }
}
