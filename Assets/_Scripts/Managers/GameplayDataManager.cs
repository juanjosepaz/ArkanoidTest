using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayDataManager : MonoBehaviour
{
    public static GameplayDataManager Instance { get; private set; }
    public static Action<int> OnLifesChanged;
    public static Action<int> OnScoreChanged;
    public static Action<int> OnHighScoreChanged;
    public const string HIGH_SCORE = "HighScore";

    [Header("Player")]
    private const int MAX_LIFES = 7;
    private int actualLifes;

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
        actualLifes = 2;
        highScore = PlayerPrefs.GetInt(HIGH_SCORE, 0);
    }

    private void Block_OnBlockDestroyed(Block block)
    {
        actualScore += block.GetBlockScore();
        OnScoreChanged?.Invoke(actualScore);

        if (actualScore > highScore)
        {
            OnHighScoreChanged?.Invoke(actualScore);
        }
    }

    public void LifeGained()
    {
        if (actualLifes + 1 < MAX_LIFES)
        {
            actualLifes++;
            OnLifesChanged?.Invoke(actualLifes);
        }
    }

    public bool LifeLost()
    {
        actualLifes--;
        OnLifesChanged?.Invoke(actualLifes);

        if (actualLifes < 0)
        {
            return false;
        }

        return true;
    }

    public (int finalScore, bool isANewHighScore) GetFinalScoreValues()
    {
        bool isANewHighScore = false;
        int lastHighScore = PlayerPrefs.GetInt(HIGH_SCORE, 0);

        if (actualScore > lastHighScore)
        {
            highScore = actualScore;
            PlayerPrefs.SetInt(HIGH_SCORE, actualScore);
            isANewHighScore = true;
        }

        return (actualScore, isANewHighScore);
    }

    public int GetActualHighScore()
    {
        if (actualScore > highScore) { return actualScore; }
        return highScore;
    }

    public int GetActualScore() => actualScore;
    public int GetActualLifes() => actualLifes;
}
