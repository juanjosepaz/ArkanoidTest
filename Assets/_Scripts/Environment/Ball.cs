using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    public static Action OnBallDestroyed;

    [Header("References")]
    [SerializeField] private float ballSpeed;
    [SerializeField] private Rigidbody2D rb2D;

    [Header("Values")]
    [Range(0f, 0.9f)][SerializeField] private float hitpointRange;
    [SerializeField] private float maxAngleBounceRadious;
    [SerializeField] private float minClampBounceValue;
    [SerializeField] private float maxClampBounceValue;
    [SerializeField] private float playerWidth;
    private Vector3 lastVelocity;
    private Vector3 lastBounceVelocity;
    private bool blockDestroyedInThisFrame;

    [Header("Shoot Ball Values")]
    [SerializeField] private float minXShootValue = 0.2f;
    [SerializeField] private float maxXShootValue = 1f;
    [SerializeField] private float minYShootValue = 0.5f;
    [SerializeField] private float maxYShootValue = 1f;
    private bool isBallMoving;

    #region UnityMethods

    private void Update()
    {
        if (!isBallMoving) { return; }

        FixVelocity();

        lastVelocity = rb2D.velocity;

        rb2D.velocity = rb2D.velocity.normalized * ballSpeed;

        blockDestroyedInThisFrame = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        lastBounceVelocity = lastVelocity;

        if (other.gameObject.CompareTag("Player"))
        {
            BallBounceFromPlayer(other);
        }
        else if (other.gameObject.TryGetComponent(out IDestructible destructible))
        {
            if (!blockDestroyedInThisFrame)
            {
                destructible.TakeHit();

                blockDestroyedInThisFrame = true;
            }

            BallBounceFromWall(other);
        }
        else
        {
            BallBounceFromWall(other);
        }
    }
    #endregion

    #region BounceBall

    private void BallBounceFromWall(Collision2D other)
    {
        Vector2 contactNormal = other.contacts[0].normal;

        Vector3 direction = Vector3.Reflect(lastVelocity.normalized, contactNormal);

        direction = ClampBounceDirection(direction);

        rb2D.velocity = direction * Mathf.Max(lastVelocity.magnitude, 0);
    }

    private void BallBounceFromPlayer(Collision2D other)
    {
        Transform playerTransform = other.transform;

        Vector3 ballPosition = transform.position;

        float hitPoint = (ballPosition.x - playerTransform.position.x) / playerWidth;

        hitPoint = Mathf.Clamp(hitPoint, -1f, 1f);

        if (hitPoint > -hitpointRange && hitPoint < hitpointRange)
        {
            BallBounceFromWall(other);

            return;
        }

        float bounceAngle = hitPoint * maxAngleBounceRadious;

        Vector2 newDirection = new Vector2(Mathf.Sin(bounceAngle * Mathf.Deg2Rad), Mathf.Cos(bounceAngle * Mathf.Deg2Rad)).normalized;

        newDirection = ClampBounceDirection(newDirection);

        rb2D.velocity = newDirection * Mathf.Max(lastVelocity.magnitude, 0);
    }

    private Vector3 ClampBounceDirection(Vector3 direction)
    {
        if (direction.x < 0)
        {
            Mathf.Clamp(direction.x, -maxClampBounceValue, -minClampBounceValue);
        }
        else
        {
            Mathf.Clamp(direction.x, minClampBounceValue, maxClampBounceValue);
        }

        if (direction.y < 0)
        {
            Mathf.Clamp(direction.y, -maxClampBounceValue, -minClampBounceValue);
        }
        else
        {
            Mathf.Clamp(direction.y, minClampBounceValue, maxClampBounceValue);
        }

        return direction;
    }

    private void FixVelocity()
    {
        float minVelocity = 0.1f;

        if (Mathf.Abs(rb2D.velocity.x) < minVelocity || Mathf.Abs(rb2D.velocity.y) < minVelocity)
        {
            rb2D.velocity = -lastBounceVelocity;
        }
    }

    #endregion

    #region ShootBall
    public void ShootBall()
    {
        rb2D.simulated = true;

        Vector2 shootDirection = GetRandomShootDirection();

        rb2D.velocity = shootDirection.normalized * ballSpeed;

        isBallMoving = true;
    }

    private Vector2 GetRandomShootDirection()
    {
        int randomXDirection = Random.Range(0, 2);

        Vector2 shootDirection = new();

        if (randomXDirection == 1)
        {
            shootDirection.x = Random.Range(minXShootValue, maxXShootValue);
        }
        else
        {
            shootDirection.x = Random.Range(-maxXShootValue, -minXShootValue);
        }

        shootDirection.y = Random.Range(minYShootValue, maxYShootValue);

        return shootDirection;
    }

    #endregion

    #region Behaviour

    public void DestroyBall()
    {
        OnBallDestroyed?.Invoke();

        Destroy(gameObject);
    }

    #endregion
}
