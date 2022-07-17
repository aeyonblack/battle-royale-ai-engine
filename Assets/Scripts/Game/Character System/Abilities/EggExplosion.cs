using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When stepped on, this egg explodes
/// </summary>
public class EggExplosion : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float explosionRadius;
    public float force;
    public float forceMultiplier;
    public float minDamage;
    public float maxDamage;
    public float timeToSelfDestruct;

    private void Start()
    {
        print("ExplosiveEgg: START");
        StartCoroutine(SelfDestructCountDown());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character") && 
            other.tag != "Player")
        {
            StartCoroutine(Explode());
        }
    }

    private IEnumerator SelfDestructCountDown()
    {
        yield return new WaitForSeconds(timeToSelfDestruct);
        yield return Explode();
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));

        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb == null) rb = hit.GetComponentInParent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(force * forceMultiplier, explosionPosition, explosionRadius);
            }

            DamageTarget(hit);
        }
        var explosion = Poolable.TryGetPoolable<Poolable>(explosionPrefab);
        explosion.transform.position = explosionPosition;
        Destroy(gameObject);
    }

    private void DamageTarget(Collider hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Character") && 
            hit.tag != "Player")
        {
            var health = hit.GetComponent<Health>();
            health?.TakeDamage(Random.Range(minDamage, maxDamage));
        }
    }

    private void ReturnToPool()
    {
        if (!gameObject.activeInHierarchy) return;
        Poolable.TryPool(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
