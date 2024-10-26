using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BallObjectPool : MonoBehaviour
{
    public static BallObjectPool Instance { get; private set; }
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private int defaultPoolCapacity;
    [SerializeField] private int maxPoolSize;
    private ObjectPool<Ball> ballsPool;

    private void Awake()
    {
        Instance = this;

        ballsPool = new ObjectPool<Ball>(() =>
        {
            Ball ball = Instantiate(ballPrefab);
            ball.ReleaseBall(ReleaseBall);
            return ball;
        }, ball =>
        {
            ball.gameObject.SetActive(true);
        }, ball =>
        {
            ball.gameObject.SetActive(false);
        }, ball =>
        {
            Destroy(ball.gameObject);
        },
            true,
            defaultPoolCapacity,
            maxPoolSize
        );
    }

    public Ball GetBall()
    {
        return ballsPool.Get();
    }

    public void ReleaseBall(Ball ball)
    {
        ballsPool.Release(ball);
    }
}
