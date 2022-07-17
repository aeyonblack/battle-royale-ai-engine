using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles how a radioactive or poisonous gas damages a character
/// </summary>
public class GasDamager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name + " has entered the gas");
        if (other.tag != "Player" &&
            other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            Debug.Log(other.gameObject.name);
            var health = other.GetComponent<Health>();
            StartCoroutine(DamageTarget(health));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /*StopAllCoroutines();*/
        // what if i used something else besides collision detection?
    }

    private IEnumerator DamageTarget(Health health)
    {
        print(health.currentHealth + " = current health");
        while(true)
        {
            health?.TakeDamage(Random.Range(15, 30));
            yield return new WaitForSeconds(0.5f);
        }
    }

}
