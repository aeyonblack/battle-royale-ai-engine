using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dragonFire", menuName = "Epic Legends/Create/DragonFireAttack")]
public class DragonFireAttack : AttackData
{

    public GameObject Flames;

    public void Fire(Transform attacker, Transform firePoint, Transform target)
    {
        var flames = Poolable.TryGetPoolable<Poolable>(Flames);
        flames.transform.position = firePoint.position;
        flames.transform.forward = firePoint.forward;
        flames.transform.SetParent(firePoint);
    }

    public void Execute(Transform attacker, Transform target)
    {
        if (target == null) return;
        if (Vector3.Distance(attacker.position, target.position) > AttackDistance) return;

        var attack = CreateAttack();
        var childHealth = target.GetComponent<Health>();
        var health = childHealth ? childHealth : target.GetComponentInParent<Health>();
        health.TakeDamage(attack.Damage);
    }  
}
