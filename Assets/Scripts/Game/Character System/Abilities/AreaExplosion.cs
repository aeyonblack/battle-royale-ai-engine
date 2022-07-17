using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaExplosion : MonoBehaviour
{
    public FixedButton AbilityButton;
    public GameObject explosionPrefab;
    public float explosionRadius = 6;
    public float force = 200;
    public float forceMultiplier = 5;
    public float minDamage = 10;
    public float maxDamage = 30;

    private void Start()
    {
        AbilityButton.ButtonPressed += () =>
        {
            StartCoroutine(Explode());
        };
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.1f);

        Vector3 explosionPosition = transform.position;
        var explosion = Poolable.TryGetPoolable<Poolable>(explosionPrefab);
        explosion.transform.position = explosionPosition;
        yield return new WaitForSeconds(0.3f);
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
    }

    private void DamageTarget(Collider hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Character") && 
            hit.tag != "Player")
        {
            var target = hit.GetComponent<Health>();
            target?.TakeDamage(Random.Range(minDamage, maxDamage));
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
