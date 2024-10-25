using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [Header("PowerUps")]
    [Range(0f, 100f)][SerializeField] private float spawnPowerUpChance;
    [SerializeField] private PowerUpBase[] powerUpPrefabs;

    private void OnEnable()
    {
        Block.OnBlockDestroyed += Block_OnBlockDestroyed;
        Ball.OnBallDestroyed += Ball_OnBallDestroyed;
        Ball.OnBallCreated += Ball_OnBallCreated;
    }

    private void OnDisable()
    {
        Block.OnBlockDestroyed -= Block_OnBlockDestroyed;
        Ball.OnBallDestroyed -= Ball_OnBallDestroyed;
        Ball.OnBallCreated -= Ball_OnBallCreated;
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
    }

    private void Ball_OnBallCreated()
    {
        ballsInGame++;
    }

    private void GetAllBlocksToDestroyInLevel()
    {
        Block[] allBlocksInLevel = FindObjectsByType<Block>(FindObjectsSortMode.None);

        int blockCount = 0;

        foreach (Block block in allBlocksInLevel)
        {
            if (block.CanDestroyTheBlock())
            {
                blockCount++;
            }
        }

        blocksInLevel = blockCount;
    }

    private void Block_OnBlockDestroyed(Block block)
    {
        blocksDestroyed++;

        if (blocksDestroyed >= blocksInLevel)
        {
            OnRoundWon?.Invoke();
        }
        else
        {
            TryToSpawnAPowerUp(block.transform.position);
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

        bool canPlayAgain = GameplayDataManager.Instance.LifeLost();

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


    private void TryToSpawnAPowerUp(Vector3 position)
    {
        float randomNumber = Random.Range(0f, 100f);

        if (randomNumber < spawnPowerUpChance)
        {
            SpawnRandomPowerUp(position);
        }
    }

    private void SpawnRandomPowerUp(Vector3 position)
    {
        int randomPowerUpIndex = Random.Range(0, powerUpPrefabs.Length);

        Instantiate(powerUpPrefabs[randomPowerUpIndex], position, Quaternion.identity);
    }

}
