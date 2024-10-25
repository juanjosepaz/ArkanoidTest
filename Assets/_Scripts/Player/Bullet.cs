using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const string BORDERS_LAYER_NAME = "Borders";
    [SerializeField] private float movementSpeed;
    [SerializeField] private GameObject destroyParticle;

    private void Update()
    {
        transform.Translate(movementSpeed * Time.deltaTime * Vector2.up);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDestructible destructible))
        {
            destructible.TakeHit();

            InstantiateDestroyParticle();

            Destroy(gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer(BORDERS_LAYER_NAME))
        {
            InstantiateDestroyParticle();
            
            Destroy(gameObject);
        }
    }

    private void InstantiateDestroyParticle()
    {
        destroyParticle.SetActive(true);

        destroyParticle.transform.SetParent(null);
    }
}
