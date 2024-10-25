using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Block : MonoBehaviour, IDestructible
{
    public static Action<Block> OnBlockDestroyed;

    [SerializeField] private int score;

    [Header("Health")]
    [SerializeField] private int maxHitsToDestroy;
    [SerializeField] private int actualHealth;
    [SerializeField] private bool canDestroyTheBlock = true;

    private void OnEnable()
    {
        actualHealth = maxHitsToDestroy;
    }

    public void TakeHit()
    {
        if (!canDestroyTheBlock) { return; }

        actualHealth--;

        if (actualHealth <= 0)
        {
            DestroyObjectBehaviour();
        }
    }

    public void DestroyObjectBehaviour()
    {
        OnBlockDestroyed?.Invoke(this);

        Destroy(gameObject);
    }

    public int GetBlockScore() => score;
    public bool CanDestroyTheBlock() => canDestroyTheBlock;
}
