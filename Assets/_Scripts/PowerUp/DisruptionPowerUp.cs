using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisruptionPowerUp : PowerUpBase
{
    [SerializeField] private float angleShootNewBalls;

    protected override void GivePlayerPowerUpOnTouch(Collider2D player)
    {
        Ball farthestBall = GetFarthestBall();

        if (farthestBall == null) { return; }

        Vector2 ballDirection = farthestBall.GetBallVelocityDirection();

        float ballAngle = DirectionAngleConverter.GetAngleFromDirection(ballDirection);

        SpawnNewBall(farthestBall.transform.position, ballAngle, angleShootNewBalls);
        SpawnNewBall(farthestBall.transform.position, ballAngle, -angleShootNewBalls);
    }

    private void SpawnNewBall(Vector2 farthestBallPosition, float farthestBallAngle, float addNewAngle)
    {
        Ball newBallRight = GetNewBall();

        newBallRight.transform.position = farthestBallPosition;

        float newBallAngleRight = farthestBallAngle + addNewAngle;

        Vector2 newBallDirectionRight = DirectionAngleConverter.GetDirectionFromAngle(newBallAngleRight);

        newBallRight.SetNewBallOnSpawn(newBallDirectionRight);
    }

    private Ball GetFarthestBall()
    {
        Ball[] allBallsInScene = FindObjectsByType<Ball>(FindObjectsSortMode.None);

        Ball farthestBall = null;

        foreach (Ball ball in allBallsInScene)
        {
            if (farthestBall == null)
            {
                farthestBall = ball;
                continue;
            }

            if (ball.transform.position.y > farthestBall.transform.position.y)
            {
                farthestBall = ball;
            }
        }

        return farthestBall;
    }

    private Ball GetNewBall()
    {
        return BallObjectPool.Instance.GetBall();
    }
}
