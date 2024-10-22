using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatform : MonoBehaviour
{
    public static Action<PlayerPlatform> OnPlayerSpawned;

    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField] private Animator animator;

    [Header("Animation Values")]
    [SerializeField] private float waitTimeToPlay;
    [SerializeField] private float waitTimeToDestroy;
    private const string DESTROY_ANIMATION_TRIGGER = "Destroy";

    private void Awake()
    {
        OnPlayerSpawned?.Invoke(this);
    }

    private void OnEnable()
    {
        StartCoroutine(GainPlayerControl());
    }

    private IEnumerator GainPlayerControl()
    {
        yield return new WaitForSeconds(waitTimeToPlay);

        playerMovement.SetPlayerCanMove(true);

        playerShoot.SetPlayerCanShoot(true);
    }

    public void LosePlayerControl()
    {
        playerMovement.SetPlayerCanMove(false);

        playerShoot.SetPlayerCanShoot(false);
    }

    public void DestroyPlayerOnDefeat()
    {
        StartCoroutine(DestroyPlayerOnDefeatCoroutine());
    }

    private IEnumerator DestroyPlayerOnDefeatCoroutine()
    {
        LosePlayerControl();

        animator.SetTrigger(DESTROY_ANIMATION_TRIGGER);

        yield return new WaitForSeconds(waitTimeToDestroy);

        Destroy(gameObject);
    }

}
