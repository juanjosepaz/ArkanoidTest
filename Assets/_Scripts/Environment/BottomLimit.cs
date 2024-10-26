using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomLimit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Ball ball))
        {
            ball.DestroyBall(true);
        }

        if (other.TryGetComponent(out PowerUpBase powerUp))
        {
            Destroy(powerUp);
        }
    }
}
