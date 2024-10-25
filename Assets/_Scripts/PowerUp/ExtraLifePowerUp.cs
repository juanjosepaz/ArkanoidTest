using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLifePowerUp : PowerUpBase
{
    protected override void GivePlayerPowerUpOnTouch(Collider2D _)
    {
        GameplayDataManager.Instance.LifeGained();
    }
}
