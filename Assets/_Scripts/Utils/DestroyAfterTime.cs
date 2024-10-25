using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float delayTimeToDestroy;

    private void Start()
    {
        Destroy(gameObject, delayTimeToDestroy);
    }
}
