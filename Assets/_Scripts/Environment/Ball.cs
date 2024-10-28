using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Action OnBallCreated;
    public static Action OnBallDestroyed;
    private Action<Ball> releaseAction;

    [Header("References")]
    [SerializeField] private float ballSpeed;
    [SerializeField] private float baseBallSpeed;
    [SerializeField] private float maxBallSpeed;
    [SerializeField] private float ballSpeedIncreaseRate;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Collider2D ballCollider;

    [Header("Values")]
    [Range(0f, 0.9f)][SerializeField] private float hitpointRange;
    [SerializeField] private float minVelocityValue;
    [SerializeField] private float maxAngleBounceRadious;
    [SerializeField] private float playerWidth;
    [SerializeField] private float ignoreCollisionWithPlayerTime;
    [SerializeField] private float maxTimeInCollisionWithPlayer;
    private float timeInCollisionWithPlayer;
    private Vector3 lastVelocity;
    private Vector3 lastBounceVelocity;
    private bool blockDestroyedInThisFrame;

    [Header("Bounce Angle")]
    [SerializeField] private Vector2 bounceRangeVectical;
    [SerializeField] private Vector2 bounceRangeHorizontalRight;
    [SerializeField] private Vector2 bounceRangeHorizontalLeft;
    [SerializeField] private float minNormalizedValueBounceFromPlayer;

    [Header("Shoot Ball Values")]
    private bool isBallMoving;

    [Header("Sounds")]
    [SerializeField] private AudioClip playerBounceSound;

    #region UnityMethods
    private void OnEnable()
    {
        OnBallCreated?.Invoke();
    }

    private void Update()
    {
        if (!isBallMoving) { return; }

        lastVelocity = rb2D.velocity;

        blockDestroyedInThisFrame = false;
    }

    private void FixedUpdate()
    {
        if (!isBallMoving) { return; }

        FixVelocity();

        rb2D.velocity = rb2D.velocity.normalized * ballSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        lastBounceVelocity = lastVelocity;

        if (other.gameObject.CompareTag("Player"))
        {
            BallBounceFromPlayer(other);

            return;
        }

        if (other.gameObject.TryGetComponent(out IDestructible destructible))
        {
            if (!blockDestroyedInThisFrame)
            {
                BallBounceFromWall(other);

                destructible.TakeHit();

                blockDestroyedInThisFrame = true;

                return;
            }
        }

        BallBounceFromWall(other);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            timeInCollisionWithPlayer += Time.deltaTime;

            if (timeInCollisionWithPlayer > maxTimeInCollisionWithPlayer)
            {
                StartCoroutine(IgnoreCollisionWithPlayerCoroutine(other));
                rb2D.velocity = GetBounceDirectionNormalizedFromPlayer(other.transform.position);
                timeInCollisionWithPlayer = 0f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && rb2D.velocity.magnitude < minVelocityValue)
        {
            rb2D.velocity = GetDirectionToFieldCenter();
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

        IncreaseBallSpeed();
    }

    private void BallBounceFromPlayer(Collision2D other)
    {
        Vector2 contactNormal = other.contacts[0].normal;

        if (contactNormal.y < 0 || Mathf.Abs(contactNormal.x) > minNormalizedValueBounceFromPlayer)
        {
            BallBounceFromWall(other);

            return;
        }

        Transform playerTransform = other.transform;

        Vector2 bounceDirectionNormalized = GetBounceDirectionNormalizedFromPlayer(playerTransform.position);

        rb2D.velocity = bounceDirectionNormalized * Mathf.Max(lastVelocity.magnitude, 0);

        StartCoroutine(IgnoreCollisionWithPlayerCoroutine(other));

        SoundManager.Instance.PlaySound(playerBounceSound);
    }

    private IEnumerator IgnoreCollisionWithPlayerCoroutine(Collision2D other)
    {
        Collider2D playerCollider = other.gameObject.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(ballCollider, playerCollider, true);

        yield return new WaitForSeconds(ignoreCollisionWithPlayerTime);

        Physics2D.IgnoreCollision(ballCollider, playerCollider, false);
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
                rb2D.velocity = GetDirectionToFieldCenter();
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

        SoundManager.Instance.PlaySound(playerBounceSound);
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

    public void InitializeBallOnPlayer(Collider2D playerCollider)
    {
        rb2D.simulated = false;
        isBallMoving = false;
        rb2D.velocity = Vector2.zero;
        lastBounceVelocity = Vector2.zero;
        lastVelocity = Vector2.zero;
        ballSpeed = baseBallSpeed;
        Physics2D.IgnoreCollision(ballCollider, playerCollider, false);
    }

    public void ReleaseBall(Action<Ball> releaseActionParameter)
    {
        releaseAction = releaseActionParameter;
    }

    public void DestroyBall(bool destroyedByLimit)
    {
        if (destroyedByLimit)
        {
            OnBallDestroyed?.Invoke();
        }

        releaseAction(this);
    }

    public void SetNewBallOnSpawn(Vector2 direction)
    {
        rb2D.simulated = true;
        rb2D.velocity = direction * ballSpeed;
        isBallMoving = true;
        ballSpeed = baseBallSpeed;
    }

    private void IncreaseBallSpeed()
    {
        if (ballSpeed + ballSpeedIncreaseRate < maxBallSpeed)
        {
            ballSpeed += ballSpeedIncreaseRate;
        }
    }

    private Vector2 GetDirectionToFieldCenter()
    {
        return (Vector3.zero - transform.position).normalized;
    }

    #endregion

    public Vector2 GetBallVelocityDirection() => rb2D.velocity.normalized;
    public Vector2 GetLastVelocity() => lastVelocity;
    public Vector2 GetLastBounceVelocity() => lastBounceVelocity;
}
