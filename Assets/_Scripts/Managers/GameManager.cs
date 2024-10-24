using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action OnRoundWon;
    public static Action OnGameOver;

    [Header("References")]
    [SerializeField] private RoundReadyAnimationUI roundReadyAnimationUI;

    [Header("Player")]
    [SerializeField] private PlayerPlatform playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private float waitTimeToSpawnPlayerAgain;
    private PlayerPlatform player;

    [Header("Balls Values")]
    private int ballsInGame;

    [Header("Block Values")]
    private int blocksInLevel;
    private int blocksDestroyed;

    private void OnEnable()
    {
        Block.OnBlockDestroyed += Block_OnBlockDestroyed;
        Ball.OnBallDestroyed += Ball_OnBallDestroyed;
    }


    private void OnDisable()
    {
        Block.OnBlockDestroyed -= Block_OnBlockDestroyed;
        Ball.OnBallDestroyed -= Ball_OnBallDestroyed;
    }

    private void Start()
    {
        StartCoroutine(StartGameSequenceRoutine());
    }

    private IEnumerator StartGameSequenceRoutine()
    {
        GetAllBlocksToDestroyInLevel();

        float timeToCompleteSceneAnimation = SceneTransitionUI.Instance.GetTimeToCompleteAnimation();

        roundReadyAnimationUI.StartRoundAnimation(timeToCompleteSceneAnimation);

        float waitPlayerAnimationTime = roundReadyAnimationUI.GetAnimationTime() + timeToCompleteSceneAnimation;

        yield return new WaitForSeconds(waitPlayerAnimationTime);

        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);

        ballsInGame = 1;
    }

    private void GetAllBlocksToDestroyInLevel()
    {
        blocksInLevel = FindObjectsByType<Block>(FindObjectsSortMode.None).Length;
    }

    private void Block_OnBlockDestroyed(int score)
    {
        blocksDestroyed++;

        if (blocksDestroyed >= blocksInLevel)
        {
            OnRoundWon?.Invoke();
        }
    }

    private void Ball_OnBallDestroyed()
    {
        ballsInGame--;

        if (ballsInGame <= 0)
        {
            Defeat();
        }
    }

    private void Defeat()
    {
        DestroyObjectsOnDefeat();

        bool canPlayAgain = GameplayDataManager.Instance.LiveLost();

        if (canPlayAgain)
        {
            StartCoroutine(PlayAgainCoroutine());
        }
        else
        {
            OnGameOver?.Invoke();
        }
    }

    private void DestroyObjectsOnDefeat()
    {
        if (player != null)
        {
            player.DestroyPlayerOnDefeat();

            player = null;
        }
    }

    private IEnumerator PlayAgainCoroutine()
    {
        yield return new WaitForSeconds(waitTimeToSpawnPlayerAgain);

        SpawnPlayer();
    }


}
