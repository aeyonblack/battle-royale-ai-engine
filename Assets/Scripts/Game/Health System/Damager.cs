using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Damager : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        OnDamage(collision);
    }

    private void OnDamage(Collision collision)
    {
        Debug.Log("Damaging...");
    }

    private void ComputeDamage()
    {
        bool isCriticalDamage = Random.Range(0, 100) < 50;
        //float criticalHit
    }
}
