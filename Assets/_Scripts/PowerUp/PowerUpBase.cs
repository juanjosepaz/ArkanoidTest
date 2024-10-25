using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class PowerUpBase : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 2.5f;

    protected abstract void GivePlayerPowerUpOnTouch(Collider2D player);

    private void Update()
    {
        transform.Translate(fallSpeed * Time.deltaTime * Vector2.down);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GivePlayerPowerUpOnTouch(other);
            Destroy(gameObject);
        }
    }
}
