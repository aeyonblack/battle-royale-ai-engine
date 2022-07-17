using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject Explosion;

    private Rigidbody rb;
    private float minDamage;
    private float maxDamage;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (rb.velocity.magnitude != 0) transform.forward = rb.velocity;
    }

    public void Launch(float speed, Transform launcher, RocketAttack attack)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.velocity = speed * launcher.forward;
        minDamage = attack.MinDamage;
        maxDamage = attack.MaxDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Rocket")
        {
            var exp = Poolable.TryGetPoolable<Poolable>(Explosion);
            exp.transform.position = other.transform.position;
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            DamageTarget(other.GetComponent<Health>());
        }
        ReturnToPool();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Rocket")
        {
            var exp = Poolable.TryGetPoolable<Poolable>(Explosion);
            exp.transform.position = collision.contacts[0].point;
        }
        ReturnToPool();
    }

    private void DamageTarget(Health target)
    {
        Debug.Log("min damage = " + minDamage);
        Debug.Log("max damage = " + maxDamage);
        target?.TakeDamage(Random.Range(minDamage, maxDamage));
    }

    private void ReturnToPool()
    {
        if (!gameObject.activeInHierarchy) return;
        Poolable.TryPool(gameObject);
    }

}
