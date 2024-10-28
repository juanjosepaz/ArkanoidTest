using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IDestructible
{
    public static Action<Block> OnBlockDestroyed;

    [SerializeField] private int score;

    [Header("Health")]
    [SerializeField] private int maxHitsToDestroy;
    [SerializeField] private int actualHealth;
    [SerializeField] private bool canDestroyTheBlock = true;
    private bool isDestroyed;

    [Header("Sounds")]
    [SerializeField] private AudioClip blockHitSound;


    private void OnEnable()
    {
        actualHealth = maxHitsToDestroy;
    }

    public void TakeHit()
    {
        SoundManager.Instance.PlaySound(blockHitSound);

        if (!canDestroyTheBlock) { return; }

        actualHealth--;

        if (actualHealth <= 0 && !isDestroyed)
        {
            DestroyObjectBehaviour();
        }
    }

    public void DestroyObjectBehaviour()
    {
        isDestroyed = true;

        OnBlockDestroyed?.Invoke(this);

        Destroy(gameObject);
    }

    public int GetBlockScore() => score;
    public bool CanDestroyTheBlock() => canDestroyTheBlock;
}
