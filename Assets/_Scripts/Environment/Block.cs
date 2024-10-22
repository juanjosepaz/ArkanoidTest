using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IDestructible
{
    public static Action OnBlockDestroyed;

    [SerializeField] private int maxHitsToDestroy;
    [SerializeField] private int actualHealth;

    private void OnEnable()
    {
        actualHealth = maxHitsToDestroy;
    }

    public void TakeHit()
    {
        actualHealth--;

        if (actualHealth <= 0)
        {
            DestroyObjectBehaviour();
        }
    }

    public void DestroyObjectBehaviour()
    {
        OnBlockDestroyed?.Invoke();

        Destroy(gameObject);
    }
}
