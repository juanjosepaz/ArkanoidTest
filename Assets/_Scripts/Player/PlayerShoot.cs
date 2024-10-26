using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform ballSpawnPoint;
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
    [SerializeField] private float powerUpTime;
    [SerializeField] private float deployTime;
    private IEnumerator powerUpTimeCoroutine;

    private void Start()
    {
        AttachInitialBallToPlayer();
    }

    private void AttachInitialBallToPlayer()
    {
        ballAttachedToPlayer = BallObjectPool.Instance.GetBall();
        ballAttachedToPlayer.InitializeBallOnPlayer();
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

        powerUpTimeCoroutine = ShootPowerUpCoroutine();

        StartCoroutine(powerUpTimeCoroutine);
    }

    private IEnumerator ShootPowerUpCoroutine()
    {
        if (!isShootPowerUpActive)
        {
            animator.SetTrigger("Deploy");

            yield return new WaitForSeconds(deployTime);
        }

        isShootPowerUpActive = true;

        yield return new WaitForSeconds(powerUpTime);

        isShootPowerUpActive = false;

        animator.SetTrigger("TurnOffLaser");

        yield return new WaitForSeconds(deployTime);

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
