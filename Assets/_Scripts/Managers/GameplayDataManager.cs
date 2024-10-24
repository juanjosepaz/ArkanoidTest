using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayDataManager : MonoBehaviour
{
    public static GameplayDataManager Instance { get; private set; }
    public const string HIGH_SCORE = "HighScore";

    [Header("Player")]
    private const int MAX_LIVES = 5;
    private int actualLives;

    [Header("Score")]
    private int highScore;
    private int actualScore;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        InitializeDataManager();
    }

    private void OnEnable()
    {
        Block.OnBlockDestroyed += Block_OnBlockDestroyed;
    }

    private void OnDisable()
    {
        Block.OnBlockDestroyed -= Block_OnBlockDestroyed;
    }

    public void InitializeDataManager()
    {
        actualScore = 0;
        actualLives = 2;
        highScore = PlayerPrefs.GetInt(HIGH_SCORE, 0);
    }

    private void Block_OnBlockDestroyed(int score)
    {
        actualScore += score;
    }

    public bool LiveLost()
    {
        actualLives--;

        if (actualLives < 0)
        {
            return false;
        }

        return true;
    }

    public (int finalScore, bool isANewHighScore) GetFinalScoreValues()
    {
        bool isANewHighScore = false;

        if (actualScore > highScore)
        {
            highScore = actualScore;
            PlayerPrefs.SetInt(HIGH_SCORE, actualScore);
            isANewHighScore = true;
        }

        return (actualScore, isANewHighScore);
    }
}
