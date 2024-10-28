using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatform : MonoBehaviour
{
    private const string DESTROY_ANIMATION_TRIGGER = "Destroy";

    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField] private Animator animator;

    [Header("Animation Values")]
    [SerializeField] private float waitTimeToPlay;
    [SerializeField] private float waitTimeToDestroy;

    [Header("Sounds")]
    [SerializeField] private AudioClip playerDestroyedSound;

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

        SoundManager.Instance.PlaySound(playerDestroyedSound);

        yield return new WaitForSeconds(waitTimeToDestroy);

        Destroy(gameObject);
    }

}
