using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Ball ballAttachedToPlayer;

    [Header("Values")]
    [SerializeField] private bool canShoot;

    [Header("Power Up Shoot")]
    [SerializeField] private bool isShootPowerUpActive;

    private void OnEnable()
    {
        inputReader.ShootEvent += HandleShoot;
    }

    private void OnDisable()
    {
        inputReader.ShootEvent -= HandleShoot;
    }

    private void HandleShoot(bool obj)
    {
        if (!canShoot) { return; }

        if (ballAttachedToPlayer != null)
        {
            ShootBallAttachedToPlayer();
            return;
        }

        if (!isShootPowerUpActive) { return; }

        Debug.Log("Try to shoot");
    }

    private void ShootBallAttachedToPlayer()
    {
        ballAttachedToPlayer.transform.SetParent(null);

        ballAttachedToPlayer.ShootBall();

        ballAttachedToPlayer = null;
    }

    public void SetPlayerCanShoot(bool canShoot)
    {
        this.canShoot = canShoot;
    }
}
