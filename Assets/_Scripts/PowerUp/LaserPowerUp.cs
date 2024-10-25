using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPowerUp : PowerUpBase
{
    [SerializeField] private float powerUpTime;

    protected override void GivePlayerPowerUpOnTouch(Collider2D player)
    {
        if (player.TryGetComponent(out PlayerShoot playerShoot))
        {
            playerShoot.ActivateShootPowerUp(powerUpTime);
        }
    }
}
