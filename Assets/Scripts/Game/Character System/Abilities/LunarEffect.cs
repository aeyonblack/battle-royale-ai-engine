using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// The lunar effect has two different behaviors
/// Behavior 1 - Player drains and absorbs health of all enemies in range
/// this behaviour is enabled by setting the absorbEnemyHealth boolean to true
/// Pros - Player gains health Cons - Ends when player health = 100% or when timer runs out
/// 
/// Behaviour 2 - Player drains enemy health but does not absorb it
/// Pros - Ends when timer has run out / longer
/// Cons - Player does not absorb health, but its oKay
/// </summary>
public class LunarEffect : MonoBehaviour
{

    public float EffectDuration;
    public float DamageRate;
    public float MinDamage = 10f;
    public float MaxDamage = 30f;
    public bool AbsorbEnemyHealth = true;

    public Action EffectEnded;

    private List<Health> targets;
    private Health health;

    private bool effectStarted = false;

    private void Start()
    {
        targets = new List<Health>();
        health = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Health>();
    }

    private void Update()
    {
        if (targets.Count > 0 && !effectStarted)
        {
            effectStarted = true;
            StartCoroutine(LunarCoroutine());
        }
    }

    private IEnumerator LunarCoroutine()
    {
        float t = 0;
        float damage;
        while (t < EffectDuration || EffectDuration == -1)
        {
            if (targets.Count > 0)
            {
                float gain = 0;
                foreach (var target in targets)
                {
                    if (target)
                    {
                        damage = Random.Range(MinDamage, MaxDamage);
                        gain += damage;
                        target.TakeDamage(damage);
                    }
                }
                if (AbsorbEnemyHealth)
                {
                    yield return new WaitForSeconds(0.1f);
                    health.ModifyHealth(gain);
                    if (health.currentHealth == health.maxHealth)
                    {
                        effectStarted = false;
                        yield break;
                    }
                }
            }
            yield return new WaitForSeconds(DamageRate);
            t += 0.5f;
        }
        EffectEnded?.Invoke();
        effectStarted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character") &&
            other.tag != "Player")
        {
            if (other.gameObject.name != gameObject.name) 
                targets.Add(other.GetComponent<Health>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character") && 
            other.tag != "Player")
        {
            if (other.gameObject.name != gameObject.name)
            {
                var target = other.GetComponent<Health>();
                if (targets.Contains(target)) targets.Remove(target);
            }
        }
    }
}
