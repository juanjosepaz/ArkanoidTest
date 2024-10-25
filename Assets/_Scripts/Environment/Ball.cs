using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    public static Action OnBallCreated;
    public static Action OnBallDestroyed;

    [Header("References")]
    [SerializeField] private float ballSpeed;
    [SerializeField] private Rigidbody2D rb2D;

    [Header("Values")]
    [Range(0f, 0.9f)][SerializeField] private float hitpointRange;
    [SerializeField] private float minVelocityValue;
    [SerializeField] private float maxAngleBounceRadious;
    [SerializeField] private float playerWidth;
    private Vector3 lastVelocity;
    private Vector3 lastBounceVelocity;
    private bool blockDestroyedInThisFrame;

    [Header("Bounce Angle")]
    [SerializeField] private Vector2 bounceRangeVectical;
    [SerializeField] private Vector2 bounceRangeHorizontalRight;
    [SerializeField] private Vector2 bounceRangeHorizontalLeft;

    [Header("Shoot Ball Values")]
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

    private void OnEnable()
    {
        OnBallCreated?.Invoke();
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

        Vector2 direction = Vector2.Reflect(lastVelocity.normalized, contactNormal);

        direction = ClampBounceDirection(direction);

        rb2D.velocity = direction * Mathf.Max(lastVelocity.magnitude, 0);
    }

    private void BallBounceFromPlayer(Collision2D other)
    {
        Transform playerTransform = other.transform;

        Vector2 bounceDirectionNormalized = GetBounceDirectionNormalizedFromPlayer(playerTransform.position);

        rb2D.velocity = bounceDirectionNormalized * Mathf.Max(lastVelocity.magnitude, 0);
    }

    private Vector3 ClampBounceDirection(Vector2 direction)
    {
        float bounceAngle = DirectionAngleConverter.GetAngleFromDirection(direction);

        float sign = Mathf.Sign(bounceAngle);

        if (Mathf.Abs(bounceAngle) > bounceRangeVectical.x && Mathf.Abs(bounceAngle) < bounceRangeVectical.y)
        {
            if (Mathf.Abs(bounceAngle - bounceRangeVectical.x) < Mathf.Abs(bounceAngle - bounceRangeVectical.y))
            {
                bounceAngle = bounceRangeVectical.x;
            }
            else
            {
                bounceAngle = bounceRangeVectical.y;
            }
        }
        else if (Mathf.Abs(bounceAngle) > bounceRangeHorizontalRight.x && Mathf.Abs(bounceAngle) < bounceRangeHorizontalRight.y)
        {
            if (Mathf.Abs(bounceAngle - bounceRangeHorizontalRight.x) < Mathf.Abs(bounceAngle - bounceRangeHorizontalRight.y))
            {
                bounceAngle = bounceRangeHorizontalRight.x;
            }
            else
            {
                bounceAngle = bounceRangeHorizontalRight.y;
            }
        }
        else if (Mathf.Abs(bounceAngle) > bounceRangeHorizontalLeft.x && Mathf.Abs(bounceAngle) < bounceRangeHorizontalLeft.y)
        {
            if (Mathf.Abs(bounceAngle - bounceRangeHorizontalLeft.x) < Mathf.Abs(bounceAngle - bounceRangeHorizontalLeft.y))
            {
                bounceAngle = bounceRangeHorizontalLeft.x;
            }
            else
            {
                bounceAngle = bounceRangeHorizontalLeft.y;
            }
        }
        else
        {
            return direction;
        }

        bounceAngle *= sign;

        Vector2 newDirection = DirectionAngleConverter.GetDirectionFromAngle(bounceAngle);

        return newDirection;
    }

    private void FixVelocity()
    {
        if (Mathf.Abs(rb2D.velocity.x) < minVelocityValue || Mathf.Abs(rb2D.velocity.y) < minVelocityValue)
        {
            if (lastBounceVelocity == Vector3.zero)
            {
                rb2D.velocity = new(-minVelocityValue, -minVelocityValue);
                lastBounceVelocity = -rb2D.velocity;
            }
            else
            {
                rb2D.velocity = -lastBounceVelocity;
            }
        }
    }

    #endregion

    #region ShootBall
    public void ShootBall(Vector3 playerPosition)
    {
        rb2D.simulated = true;

        Vector2 shootDirection = GetBounceDirectionNormalizedFromPlayer(playerPosition);

        rb2D.velocity = shootDirection * ballSpeed;

        isBallMoving = true;
    }

    private Vector2 GetBounceDirectionNormalizedFromPlayer(Vector3 playerPosition)
    {
        Vector3 ballPosition = transform.position;

        float hitPoint = (ballPosition.x - playerPosition.x) / playerWidth;

        hitPoint = Mathf.Clamp(hitPoint, -1f, 1f);

        float bounceAngle = hitPoint * maxAngleBounceRadious;

        Vector2 newDirection = new Vector2(Mathf.Sin(bounceAngle * Mathf.Deg2Rad), Mathf.Cos(bounceAngle * Mathf.Deg2Rad)).normalized;

        newDirection = ClampBounceDirection(newDirection);

        return newDirection.normalized;
    }

    #endregion

    #region Behaviour

    public void DestroyBall()
    {
        OnBallDestroyed?.Invoke();

        Destroy(gameObject);
    }

    #endregion

    public void SetBallVelocityByDirection(Vector2 direction)
    {
        rb2D.simulated = true;
        rb2D.velocity = direction * ballSpeed;
        isBallMoving = true;
    }

    public Vector2 GetBallVelocityDirection() => rb2D.velocity.normalized;
    public Vector2 GetLastVelocity() => lastVelocity;
    public Vector2 GetLastBounceVelocity() => lastBounceVelocity;
}
