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
    private const int MAX_LIFES = 6;
    private int actualLifes;

    [Header("Score")]
    private int highScore;
    private int actualScore;
    private int savedScore;

    [Header("Sounds")]
    [SerializeField] private AudioClip extraLifeSound;

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
        GameManager.OnRoundWon += GameManager_OnRoundWon;
    }

    private void OnDisable()
    {
        Block.OnBlockDestroyed -= Block_OnBlockDestroyed;
        GameManager.OnRoundWon -= GameManager_OnRoundWon;
    }

    public void InitializeDataManager()
    {
        savedScore = 0;
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

    private void GameManager_OnRoundWon()
    {
        savedScore = actualScore;
    }

    public void LifeGained()
    {
        if (actualLifes + 1 <= MAX_LIFES)
        {
            actualLifes++;
            SoundManager.Instance.PlaySound(extraLifeSound);
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

    public (int finalScore, bool isANewHighScore) GetScoreValues()
    {
        bool isANewHighScore = CheckNewHighScore();
        return (savedScore, isANewHighScore);
    }

    private bool CheckNewHighScore()
    {
        int lastHighScore = PlayerPrefs.GetInt(HIGH_SCORE, 0);
        actualScore = savedScore;

        if (savedScore > lastHighScore)
        {
            highScore = savedScore;
            PlayerPrefs.SetInt(HIGH_SCORE, highScore);
            return true;
        }

        return false;
    }

    public void ResetScoreOnLevelReset()
    {
        actualScore = savedScore;
    }

    public int GetActualHighScore()
    {
        if (savedScore > highScore) { return savedScore; }
        return highScore;
    }

    public int GetActualScore() => savedScore;
    public int GetActualLifes() => actualLifes;
}
