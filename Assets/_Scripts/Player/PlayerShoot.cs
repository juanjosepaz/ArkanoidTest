using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private const string PLAYER_DEPLOY_ANIMATION_VALUE = "Deploy";
    private const string PLAYER_TURN_OFF_ANIMATION_VALUE = "TurnOffLaser";

    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform ballSpawnPoint;
    [SerializeField] private Collider2D playerCollider;
    private Ball ballAttachedToPlayer;

    [Header("Values")]
    [SerializeField] private bool canShoot;

    [Header("Shoot")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform[] shootPoints;
    [SerializeField] private float timeBtwnShots;
    private float lastShootTime;

    [Header("Power Up Shoot")]
    [SerializeField] private bool isShootPowerUpActive;
    [SerializeField] private float deployTime;
    private bool isDeployed;
    private IEnumerator powerUpTimeCoroutine;

    [Header("Sounds")]
    [SerializeField] private AudioClip deploySound;
    [SerializeField] private AudioClip shootSound;

    private void Start()
    {
        AttachInitialBallToPlayer();
    }

    private void AttachInitialBallToPlayer()
    {
        ballAttachedToPlayer = BallObjectPool.Instance.GetBall();
        ballAttachedToPlayer.InitializeBallOnPlayer(playerCollider);
        ballAttachedToPlayer.transform.position = ballSpawnPoint.position;
        ballAttachedToPlayer.transform.SetParent(transform);
    }

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

        TryToShoot();
    }

    private void ShootBallAttachedToPlayer()
    {
        ballAttachedToPlayer.transform.SetParent(null);

        ballAttachedToPlayer.ShootBall(transform.position);

        ballAttachedToPlayer = null;
    }

    public void ActivateShootPowerUp(float powerUpTime)
    {
        if (powerUpTimeCoroutine != null)
        {
            StopCoroutine(powerUpTimeCoroutine);
        }

        powerUpTimeCoroutine = ShootPowerUpCoroutine(powerUpTime);

        StartCoroutine(powerUpTimeCoroutine);
    }

    private IEnumerator ShootPowerUpCoroutine(float powerUpTime)
    {
        if (!isDeployed)
        {
            isDeployed = true;

            animator.SetTrigger(PLAYER_DEPLOY_ANIMATION_VALUE);

            SoundManager.Instance.PlaySound(deploySound);

            yield return new WaitForSeconds(deployTime);
        }

        isShootPowerUpActive = true;

        yield return new WaitForSeconds(powerUpTime);

        isShootPowerUpActive = false;

        animator.SetTrigger(PLAYER_TURN_OFF_ANIMATION_VALUE);

        yield return new WaitForSeconds(deployTime);

        isDeployed = false;

        powerUpTimeCoroutine = null;
    }

    private void TryToShoot()
    {
        if (Time.time > lastShootTime + timeBtwnShots)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        lastShootTime = Time.time;

        foreach (Transform shootPoint in shootPoints)
        {
            Bullet bullet = GetBullet();

            bullet.transform.position = shootPoint.position;
        }

        SoundManager.Instance.PlaySound(shootSound);
    }

    private Bullet GetBullet()
    {
        return Instantiate(bulletPrefab);
    }

    public void SetPlayerCanShoot(bool canShoot)
    {
        this.canShoot = canShoot;
    }
}
