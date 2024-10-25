using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisruptionPowerUp : PowerUpBase
{
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private float angleShootNewBalls;

    protected override void GivePlayerPowerUpOnTouch(Collider2D player)
    {
        Ball farthestBall = GetFarthestBall();

        if (farthestBall == null) { return; }

        Vector2 ballDirection = farthestBall.GetBallVelocityDirection();

        float ballAngle = DirectionAngleConverter.GetAngleFromDirection(ballDirection);

        Ball newBallRight = CreateNewBall();

        newBallRight.transform.position = farthestBall.transform.position;

        float newBallAngleRight = ballAngle + angleShootNewBalls;

        Vector2 newBallDirectionRight = DirectionAngleConverter.GetDirectionFromAngle(newBallAngleRight);

        newBallRight.SetBallVelocityByDirection(newBallDirectionRight);


        Ball newBallLeft = CreateNewBall();

        newBallLeft.transform.position = farthestBall.transform.position;

        float newBallAngleLeft = ballAngle - angleShootNewBalls;

        Vector2 newBallDirectionLeft = DirectionAngleConverter.GetDirectionFromAngle(newBallAngleLeft);

        newBallLeft.SetBallVelocityByDirection(newBallDirectionLeft);
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

    private Ball CreateNewBall()
    {
        return Instantiate(ballPrefab);
    }
}
