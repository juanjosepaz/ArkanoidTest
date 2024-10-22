using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
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
        PlayerPlatform.OnPlayerSpawned += PlayerPlatform_OnPlayerSpawned;
    }


    private void OnDisable()
    {
        Block.OnBlockDestroyed -= Block_OnBlockDestroyed;
        Ball.OnBallDestroyed -= Ball_OnBallDestroyed;
        PlayerPlatform.OnPlayerSpawned -= PlayerPlatform_OnPlayerSpawned;
    }

    private void Start()
    {
        blocksInLevel = FindObjectsByType<Block>(FindObjectsSortMode.None).Length;
        ballsInGame = 1;
    }

    private void Block_OnBlockDestroyed()
    {
        blocksDestroyed++;

        if (blocksDestroyed >= blocksInLevel)
        {
            Debug.Log("Victoria");
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

    private void PlayerPlatform_OnPlayerSpawned(PlayerPlatform platform)
    {
        player = platform;
    }

    private void Defeat()
    {
        Debug.Log("Derrota");

        if (player != null)
        {
            player.DestroyPlayerOnDefeat();

            player = null;
        }
    }
}
